using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.BobStorage
{
    public class BlobStorage : IBobStorage
    {
        public AzureStorageConfig _storageConfig;
        public BlobStorage(IOptions<AzureStorageConfig> azureStorageConfig)
        {
            _storageConfig = azureStorageConfig.Value;
        }
        private async Task<BlobContainerClient> GetBlobContainerClient(string containerName)
        {
            BlobContainerClient container = new BlobContainerClient(_storageConfig.ConnectionString, containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);

            return container;
        }
    
        public async Task<string> UploadBob(Stream file,string containerName, string blobRef)
        {
            BlobContainerClient containerClient = await GetBlobContainerClient(containerName);
            BlobClient blob = containerClient.GetBlobClient(blobRef);

            await blob.UploadAsync(file);
            return $"{_storageConfig.Url}{containerName}/{blobRef}";
        }

        public Task<string> MoveBob(string blob, string newBlobRef)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteBlob(string containerName, string blobRef)
        {
            BlobContainerClient container = await GetBlobContainerClient(containerName);
            BlobClient blob = container.GetBlobClient(blobRef);
            
            return await blob.DeleteIfExistsAsync();
        }

        public Task<string> GetFilesByRef(string blobRef)
        {
            throw new System.NotImplementedException();
        }
    }
}