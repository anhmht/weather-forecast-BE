using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GloboWeather.WeatherManagement.Application.Models.Media;
using GloboWeather.WeatherManagement.Infrastructure.Utils;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.Media
{
    public class VideoService : IVideoService
    {
        private const string AdaptiveStreamingTransformName = "Custom_H264_Layer";
        private const string CustomTransform = "Custom_H264_3Layer";
        public MediaVideoSettings VideoSettings;

        public VideoService(IOptions<MediaVideoSettings> mediaVideoSettings)
        {
            VideoSettings = mediaVideoSettings.Value;
        }

        public async Task<IList<string>> UploadVideoAsync(IFormFile file)
        {
            IAzureMediaServicesClient client;
            try
            {
                client = await Authentication.CreateMediaServicesClientAsync(VideoSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.LongRunningOperationRetryTimeout = 2;
            string uniqueness = Guid.NewGuid().ToString("N");
            string jobName = $"job-{uniqueness}";
            string locatorName = $"locator-{uniqueness}";
            string outputAssetName = $"output-{uniqueness}";
            string inputAssetName = $"input-{uniqueness}";

            _ = await GetOrCreateTransformAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                AdaptiveStreamingTransformName);

            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                _ = await CreateInputAssetAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                    inputAssetName,
                    file.FileName, stream);
            }

            _ = new JobInputAsset(assetName: inputAssetName);
            Asset outputAsset =
                await CreateOutputAssetAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                    outputAssetName);

            _ = await SubmitJobAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                AdaptiveStreamingTransformName, jobName, inputAssetName, outputAsset.Name);

            Job job = await WaitForJobToFinishAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                AdaptiveStreamingTransformName, jobName);

            if (job.State == JobState.Finished)
            {
                StreamingLocator locator = await CreateDownloadLocatorAsync(client, VideoSettings.ResourceGroup,
                    VideoSettings.AccountName, outputAsset.Name, locatorName);
                IList<string> urls = await GetMp4UrlsAsync(client, VideoSettings.ResourceGroup,
                    VideoSettings.AccountName, locator.Name);

                return urls;
            }

            return new List<string>();
        }

        public async Task<IList<string>> UploadVideoSocialAsync(IFormFile file)
        {
            IAzureMediaServicesClient client;
            try
            {
                client = await Authentication.CreateMediaServicesClientAsync(VideoSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.LongRunningOperationRetryTimeout = 2;
            string uniqueness = Guid.NewGuid().ToString("N");
            string jobName = $"job-{uniqueness}";
            string locatorName = $"locator-{uniqueness}";
            string outputAssetName = $"output-{uniqueness}";
            string inputAssetName = $"input-{uniqueness}";

            _ = await CreateCustomTransform(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                CustomTransform);

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                _ = await CreateInputAssetAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                    inputAssetName,
                    file.FileName, stream);
            }

            _ = new JobInputAsset(assetName: inputAssetName);
            Asset outputAsset =
                await CreateOutputAssetAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                    outputAssetName);

            _ = await SubmitJobAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                AdaptiveStreamingTransformName, jobName, inputAssetName, outputAsset.Name);

            Job job = await WaitForJobToFinishAsync(client, VideoSettings.ResourceGroup, VideoSettings.AccountName,
                AdaptiveStreamingTransformName, jobName);

            if (job.State == JobState.Finished)
            {
                StreamingLocator locator = await CreateStreamingLocatorAsync(client, VideoSettings.ResourceGroup,
                    VideoSettings.AccountName, outputAsset.Name, locatorName);
                IList<string> urls = await GetDashStreamingUrlsAsync(client, VideoSettings.ResourceGroup,
                    VideoSettings.AccountName, locator.Name);

                return urls;
            }

            return new List<string>();
        }


        private async Task<Asset> CreateInputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string assetName,
            string fileUpload,
            Stream stream)
        {
            var asset = await client.Assets.GetAsync(resourceGroupName, accountName, assetName);
            if (asset == null)
            {
                await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset());
            }


            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the expiration time for the SAS URL.
            var response = await client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            BlobContainerClient containerClient = new(sasUri);
            BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(fileUpload));

            BlobUploadOptions options = new BlobUploadOptions()
            {
                TransferOptions = new StorageTransferOptions
                {
                    MaximumTransferSize = 4 * 1024 * 1024,
                    InitialTransferSize = 4 * 1024 * 1024
                }
            };
            await blobClient.UploadAsync(stream, options);

            return asset;
        }

        private async Task<Asset> CreateOutputAssetAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string assetName)
        {
            Asset outputAsset = await client.Assets.GetAsync(resourceGroupName, accountName, assetName);
            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (outputAsset != null)
            {
                string unitqueness = $"-{Guid.NewGuid():N}";
                outputAssetName += unitqueness;
            }

            return await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, outputAssetName, asset);
        }

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
                Console.WriteLine("Creating a custom transform...");
                // Create a new Transform Outputs array - this defines the set of outputs for the Transform
                TransformOutput[] outputs = new TransformOutput[]
                {
                    // Create a new TransformOutput with a custom Standard Encoder Preset
                    // This demonstrates how to create custom codec and layer output settings

                    new TransformOutput(
                        new StandardEncoderPreset(
                            codecs: new Codec[]
                            {
                                // Add an AAC Audio layer for the audio encoding
                                new AacAudio(
                                    channels: 2,
                                    samplingRate: 48000,
                                    bitrate: 128000,
                                    profile: AacAudioProfile.AacLc
                                ),
                                // Next, add a H264Video for the video encoding
                                new H264Video(
                                    // Set the GOP interval to 2 seconds for all H264Layers
                                    keyFrameInterval: TimeSpan.FromSeconds(2),
                                    // Add H264Layers. Assign a label that you can use for the output filename
                                    layers: new H264Layer[]
                                    {
                                        new H264Layer(
                                            bitrate: 6670000, // Units are in bits per second and not kbps or Mbps - 3.6 Mbps or 3,600 kbps
                                            width: "1920",
                                            height: "1080",
                                            label: "HD-6670kbps" // This label is used to modify the file name in the output formats
                                        ),
                                    }
                                ),
                                // Also generate a set of PNG thumbnails
                                new PngImage(
                                    start: "25%",
                                    step: "25%",
                                    range: "80%",
                                    layers: new PngLayer[]
                                    {
                                        new PngLayer(
                                            width: "50%",
                                            height: "50%"
                                        )
                                    }
                                )
                            },
                            // Specify the format for the output files - one for video+audio, and another for the thumbnails
                            formats: new Format[]
                            {
                                // Mux the H.264 video and AAC audio into MP4 files, using basename, label, bitrate and extension macros
                                // Note that since you have multiple H264Layers defined above, you have to use a macro that produces unique names per H264Layer
                                // Either {Label} or {Bitrate} should suffice

                                new Mp4Format(
                                    filenamePattern: "Video-{Basename}-{Label}-{Bitrate}{Extension}"
                                ),
                                new PngFormat(
                                    filenamePattern: "Thumbnail-{Basename}-{Index}{Extension}"
                                )
                            }
                        ),
                        onError: OnErrorType.StopProcessingJob,
                        relativePriority: Priority.Normal
                    )
                };

                string description = "Create custom transform";
                // Create the custom Transform with the outputs defined above
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName,
                    outputs, description);
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

        private async Task<StreamingLocator> CreateDownloadLocatorAsync(
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
                    StreamingPolicyName = PredefinedStreamingPolicy.DownloadOnly
                });

            return locator;
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
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                });

            return locator;
        }

        private async Task<IList<string>> GetMp4UrlsAsync(
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

            ListPathsResponse paths =
                await client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

            foreach (var path in paths.DownloadPaths.Where(_ => _.Contains(".mp4")))
            {
                UriBuilder uriBuilder = new UriBuilder()
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,
                    Path = path
                };
                streamingUrls.Add(uriBuilder.ToString());
            }

            return streamingUrls;
        }

        private async Task<IList<string>> GetDashStreamingUrlsAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            String locatorName)
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

            ListPathsResponse paths =
                await client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

            foreach (StreamingPath path in paths.StreamingPaths)
            {
                foreach (var streamingFormatPath in path.Paths)
                {
                    UriBuilder uriBuilder = new()
                    {
                        Scheme = "https",
                        Host = streamingEndpoint.HostName,
                        Path = streamingFormatPath
                    };
                    
                    if (path.StreamingProtocol == StreamingPolicyStreamingProtocol.Dash ||
                        path.StreamingProtocol == StreamingPolicyStreamingProtocol.Hls)
                        streamingUrls.Add(uriBuilder.ToString());
                }
            }

            return streamingUrls;
        }


        #region EnsureTransformExists

        /// <summary>
        /// If the specified transform exists, return that transform. If the it does not
        /// exist, creates a new transform with the specified output. In this case, the
        /// output is set to encode a video using a custom preset.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The transform name.</param>
        /// <returns></returns>
        private async Task<Transform> CreateCustomTransform(IAzureMediaServicesClient client, string resourceGroupName,
            string accountName, string transformName)
        {
            // Does a transform already exist with the desired name? Assume that an existing Transform with the desired name
            // also uses the same recipe or Preset for processing content.
            Transform transform = client.Transforms.Get(resourceGroupName, accountName, transformName);

            if (transform == null)
            {
                Console.WriteLine("Creating a custom transform...");
                // Create a new Transform Outputs array - this defines the set of outputs for the Transform
                TransformOutput[] outputs = new TransformOutput[]
                {
                    // Create a new TransformOutput with a custom Standard Encoder Preset
                    // This demonstrates how to create custom codec and layer output settings

                    new TransformOutput(
                        new StandardEncoderPreset(
                            codecs: new Codec[]
                            {
                                // Add an AAC Audio layer for the audio encoding
                                new AacAudio(
                                    channels: 2,
                                    samplingRate: 48000,
                                    bitrate: 128000,
                                    profile: AacAudioProfile.AacLc
                                ),
                                // Next, add a H264Video for the video encoding
                                new H264Video(
                                    // Set the GOP interval to 2 seconds for all H264Layers
                                    keyFrameInterval: TimeSpan.FromSeconds(2),
                                    // Add H264Layers. Assign a label that you can use for the output filename
                                    layers: new H264Layer[]
                                    {
                                        new H264Layer(
                                            bitrate: 3600000, // Units are in bits per second and not kbps or Mbps - 3.6 Mbps or 3,600 kbps
                                            width: "1280",
                                            height: "720",
                                            label: "HD-3600kbps" // This label is used to modify the file name in the output formats
                                        ),
                                        new H264Layer(
                                            bitrate: 1600000, // Units are in bits per second and not kbps or Mbps - 1.6 Mbps or 1600 kbps
                                            width: "960",
                                            height: "540",
                                            label: "SD-1600kbps" // This label is used to modify the file name in the output formats
                                        ),
                                        new H264Layer(
                                            bitrate: 600000, // Units are in bits per second and not kbps or Mbps - 0.6 Mbps or 600 kbps
                                            width: "640",
                                            height: "360",
                                            label: "SD-600kbps" // This label is used to modify the file name in the output formats
                                        ),
                                    }
                                ),
                                // Also generate a set of PNG thumbnails
                                new PngImage(
                                    start: "25%",
                                    step: "25%",
                                    range: "80%",
                                    layers: new PngLayer[]
                                    {
                                        new PngLayer(
                                            width: "50%",
                                            height: "50%"
                                        )
                                    }
                                )
                            },
                            // Specify the format for the output files - one for video+audio, and another for the thumbnails
                            formats: new Format[]
                            {
                                // Mux the H.264 video and AAC audio into MP4 files, using basename, label, bitrate and extension macros
                                // Note that since you have multiple H264Layers defined above, you have to use a macro that produces unique names per H264Layer
                                // Either {Label} or {Bitrate} should suffice

                                new Mp4Format(
                                    filenamePattern: "Video-{Basename}-{Label}-{Bitrate}{Extension}"
                                ),
                                new PngFormat(
                                    filenamePattern: "Thumbnail-{Basename}-{Index}{Extension}"
                                )
                            }
                        ),
                        onError: OnErrorType.StopProcessingJob,
                        relativePriority: Priority.Normal
                    )
                };

                string description = "A simple custom encoding transform with 2 MP4 bitrates";
                // Create the custom Transform with the outputs defined above
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName,
                    outputs, description);
            }

            return transform;
        }

        #endregion EnsureTransformExists


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
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            List<string> assetNames,
            string contentKeyPolicyName = null)
        {
            await client.Jobs.DeleteAsync(resourceGroupName, accountName, transformName, jobName);
            foreach (var assetName in assetNames)
            {
                await client.Assets.DeleteAsync(resourceGroupName, accountName, accountName);
            }

            if (contentKeyPolicyName != null)
            {
                client.ContentKeyPolicies.Delete(resourceGroupName, accountName, contentKeyPolicyName);
            }
        }
    }
}