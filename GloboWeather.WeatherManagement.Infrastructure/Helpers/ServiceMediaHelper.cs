using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;

namespace GloboWeather.WeatherManagement.Infrastructure.Helpers
{
    public class ServiceMediaHelper
    {
        private async Task<Transform> GetOrCreateTransformAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName
        )
        {
            Transform transform = await client.Transforms.GetAsync(resourceGroupName, accountName, transformName);
            if (transform == null)
            {
                TransformOutput[] outputs = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName,
                    outputs);
            }

            return transform;
        }

        private async Task<Job> SubmitJobAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName)
        {
            JobInput jobInput = new JobInputAsset(inputAssetName);

            JobOutput[] jobOutputs =
            {
                new JobOutputAsset(outputAssetName),
            };

            Job job = await client.Jobs.CreateAsync(
                resourceGroupName,
                accountName,
                transformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs
                });

            return job;
        }

        private async Task<Job> WaitForJobToFinishAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName)
        {
            const int SleepIntervalMs = 20 * 1000;
            Job job;
            do
            {
                job = await client.Jobs.GetAsync(resourceGroupName, accountName, transformName, jobName);

                if (job.State != JobState.Finished && job.State != JobState.Error && job.State != JobState.Canceled)
                {
                    await Task.Delay(SleepIntervalMs);
                }
            } while (job.State != JobState.Finished && job.State != JobState.Error && job.State != JobState.Canceled);

            return job;
        }

        private async Task<StreamingLocator> CreateStreamingLocatorAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string locatorName)
        {
            StreamingLocator locator = await client.StreamingLocators.CreateAsync(
                resourceGroup,
                accountName,
                locatorName,
                new StreamingLocator
                {
                    AssetName = assetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.DownloadAndClearStreaming
                });

            return locator;
        }

        private async Task<IList<string>> GetStreamingUrlsAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string locatorName
        )
        {
            const string DefaultStreamingEndpointName = "default";
            IList<string> streamingUrls = new List<string>();

            StreamingEndpoint streamingEndpoint =
                await client.StreamingEndpoints.GetAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);
            if (streamingEndpoint != null)
            {
                if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
                {
                    await client.StreamingEndpoints.StartAsync(resourceGroupName, accountName,
                        DefaultStreamingEndpointName);
                }
                
            }

            ListPathsResponse paths = await client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

            foreach (var path in paths.StreamingPaths)
            {
                UriBuilder uriBuilder = new UriBuilder()
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,
                    Path = path.Paths[0]
                };
                streamingUrls.Add(uriBuilder.ToString());
            }

            return streamingUrls;
        }

        /// <summary>
        ///  Downloads the results from the specified output asset, so you can see what you got.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The output asset.</param>
        /// <param name="outputFolderName">The name of the folder into which to download the results.</param>
        // <DownloadResults>
        private async Task DownloadOutputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string outputFolderName)
        {
            if (!Directory.Exists(outputFolderName))
            {
                Directory.CreateDirectory(outputFolderName);
            }

            AssetContainerSas assetContainerSas = await client.Assets.ListContainerSasAsync(
                resourceGroup,
                accountName,
                accountName,
                permissions: AssetContainerPermission.Read,
                expiryTime: DateTime.UtcNow.AddHours(1).ToUniversalTime());
            Uri containerSasUrl = new Uri(assetContainerSas.AssetContainerSasUrls.FirstOrDefault());
            BlobContainerClient containerClient = new BlobContainerClient(containerSasUrl);

            string directory = Path.Combine(outputFolderName, assetName);
            Directory.CreateDirectory(directory);

            string continuationToken = null;
            IList<Task> downloadTasks = new List<Task>();

            do
            {
                var resultSegment = containerClient.GetBlobs().AsPages(continuationToken);
                foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        var blobClient = containerClient.GetBlobClient(blobItem.Name);
                        string fileName = Path.Combine(directory, blobClient.Name);

                        downloadTasks.Add(blobClient.DownloadToAsync(fileName));
                    }

                    continuationToken = blobPage.ContinuationToken;
                }
            } while (continuationToken != "");

            await Task.WhenAll(downloadTasks);
        }

        
        /// <summary>
        /// Deletes the jobs, assets and potentially the content key policy that were created.
        /// Generally, you should clean up everything except objects 
        /// that you are planning to reuse (typically, you will reuse Transforms, and you will persist output assets and StreamingLocators).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="accountName"></param>
        /// <param name="transformName"></param>
        /// <param name="jobName"></param>
        /// <param name="assetNames"></param>
        /// <param name="contentKeyPolicyName"></param>
        /// <returns></returns>
        // <CleanUp>
        private async Task CleanUpAsync(
            IAzureMediaServicesClient client,
            string resouceGroupName,
            string accountName,
            string transformName,
            string jobName,
            List<string> assetNames,
            string contentKeyPolicyName = null)
        {
            await client.Jobs.DeleteAsync(resouceGroupName, accountName, transformName, jobName);
            foreach (var assetName in assetNames)
            {
                await client.Assets.DeleteAsync(resouceGroupName, accountName, accountName);
            }

            if (contentKeyPolicyName != null)
            {
                client.ContentKeyPolicies.Delete(resouceGroupName, accountName, contentKeyPolicyName);
            }
        }
    }
}