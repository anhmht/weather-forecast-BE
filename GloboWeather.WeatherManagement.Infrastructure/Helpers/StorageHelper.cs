using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Infrastructure.Helpers
{
    public static class StorageHelper
    {
        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] {".jpg", ".png", ".gif", ".jpeg"};

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<string>
            UploadFileToStorage(Stream fileStream, string fileName,
                AzureStorageConfig _storageConfig)
        {
            //  fileName = Guid.NewGuid().ToString() + fileName;
            string url = "https://" +
                         _storageConfig.AccountName +
                         ".blob.core.windows.net/" +
                         _storageConfig.TempContainer + "/" + fileName;

            // Create a URI to the blob
            Uri blobUri = new Uri(url);

            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);


            // Upload the file
            await blobClient.UploadAsync(fileStream);
            //  var stringUrl =  await  CopyFileToContainerImages(fileName, _storageConfig);
            return await Task.FromResult(url);
        }

        public static async Task<List<string>> GetImageUrls(AzureStorageConfig _storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();
            //   List<ImageResponse> imageResponses = new List<ImageResponse>();

            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI
            //   BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);
            //await  blobServiceClient.DeleteBlobContainerAsync(_storageConfig.TempContainer);
            // Get reference to the container

            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.TempContainer);

            if (container.Exists())
            {
                foreach (BlobItem blobItem in container.GetBlobs())
                {
                    thumbnailUrls.Add(container.Uri + "/" + blobItem.Name);
                }
            }

            return await Task.FromResult(thumbnailUrls);
        }

        public static async Task<bool> DeleteBlobInPostContainerAsync(AzureStorageConfig _storageConfig,
            string eventId)
        {
            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri, storageCredentials);
            
            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.PostContainer);

            //  blobClient.Delete()
            if (container.Exists())
            {
                var blob = container.GetBlobs(prefix: eventId);
                foreach (var blobItem in blob)
                {
                    await container.DeleteBlobIfExistsAsync(blobItem.Name);
                }
            }

            return await Task.FromResult(true);
        }
        
        
        public static async Task<bool> DeleteBlobInPostContainerByNameAsync(AzureStorageConfig _storageConfig,
            List<string> eventUrls, string eventId)
        {
            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            List<string> eventNames = new List<string>();

            foreach (var url in eventUrls)
            {
                var urls = url.Split('/');
                var eventName = urls[urls.Length-3] +"/" + urls[urls.Length-2] + "/" +  urls[urls.Length-1];
                eventNames.Add(eventName);
            }
            // Create BlobServiceClient from the account URI

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri, storageCredentials);
            
            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.PostContainer);

            //  blobClient.Delete()
            if (container.Exists())
            {
                var blob = container.GetBlobs(prefix: eventId);
                foreach (var blobItem in blob)
                {
                    if (eventNames.Contains(blobItem.Name))
                    {
                       await container.DeleteBlobIfExistsAsync(blobItem.Name);
                    }
                }
            }

            return await Task.FromResult(true);
        }
        
        public static async Task<bool> DeleteBlobsInTempContainerAsync(AzureStorageConfig _storageConfig)
        {
            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri, storageCredentials);
            
            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.TempContainer);

            //  blobClient.Delete()
            if (container.Exists())
            {
                var blobs = container.GetBlobs();
                foreach (var blobItem in blobs)
                {
                    await container.DeleteBlobIfExistsAsync(blobItem.Name);
                }
            }

            return await Task.FromResult(true);
        }


        public static async Task<string> CopyFileToContainerPost(
            string fileName,
            string eventId,
            string folderName,
            AzureStorageConfig _storageConfig)
        {
            var stringUls = fileName.Split('/');

            string urlImage = $"https://{_storageConfig.AccountName}.blob.core.windows.net" +
                              $"/{_storageConfig.PostContainer}" +
                              $"/{eventId}" +
                              $"/{folderName}" +
                              $"/{stringUls[stringUls.Length - 1].Replace(" ", String.Empty)}";

            // Create a URI to the blob
            Uri blobUri = new Uri(urlImage);
            Uri blobUriTemp = new Uri(fileName);

            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);
            await blobClient.StartCopyFromUriAsync(blobUriTemp);

            return await Task.FromResult(urlImage);
        }
    }
}