using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GloboWeather.WeatherManagement.Application.Models.Media;
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
            var stringUrl =  await  CopyFileToContainerImages(fileName, _storageConfig);
            return await Task.FromResult(stringUrl);
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
        
        public static async Task<bool> DeleteBlobInTempsAsync(AzureStorageConfig _storageConfig)
        {
            List<string> thumbnailUrls = new List<string>();
            //   List<ImageResponse> imageResponses = new List<ImageResponse>();

            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);

            //await  blobServiceClient.DeleteBlobContainerAsync(_storageConfig.TempContainer);
            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.TempContainer);
            
          //  blobClient.Delete()
            if (container.Exists())
            {
                foreach (BlobItem blobItem in  container.GetBlobs())
                {
                  await  container.DeleteBlobAsync(blobItem.Name);
                }
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
            
        }
        
        
        public static async Task<string> CopyFileToContainerImages(
                string fileName,
                AzureStorageConfig _storageConfig)
        {
            //  fileName = Guid.NewGuid().ToString() + fileName;
            string urlTemp = $"https://{_storageConfig.AccountName}.blob.core.windows.net" +
                              $"/{ _storageConfig.TempContainer}" +
                              $"/{fileName}";
            
            string urlImage = $"https://{_storageConfig.AccountName}.blob.core.windows.net" +
                              $"/{ _storageConfig.ImageContainer}" +
                              $"/{_storageConfig.EventContainer}" +
                              $"/{DateTime.Today}/{fileName}";
        
            // Create a URI to the blob
            Uri blobUri = new Uri(urlImage);
            Uri blobUriTemp = new Uri(urlTemp);

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