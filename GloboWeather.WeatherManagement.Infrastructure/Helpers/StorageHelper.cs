using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
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
            fileName = UnitCodeToAsciiString(fileName);
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
        
        
        public static async Task<string>
            UploadAvartarToStorage(Stream fileStream, string fileName,
                AzureStorageConfig _storageConfig)
        {
            fileName = UnitCodeToAsciiString(fileName);
            //  fileName = Guid.NewGuid().ToString() + fileName;
            string url = "https://" +
                         _storageConfig.AccountName +
                         ".blob.core.windows.net/" +
                         _storageConfig.UserContainer + "/" + fileName;

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
        
        
        public static async Task<bool> DeleteBlobInPostContainerByNameAsync(AzureStorageConfig storageConfig,
            List<string> eventUrls, string eventId)
        {
            return await DeleteBlobInContainerByNameAsync(storageConfig, eventUrls, eventId,
                storageConfig.PostContainer);
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
            AzureStorageConfig storageConfig)
        {
            return await CopyFileToContainerAsync(fileName, eventId, folderName, storageConfig, storageConfig.PostContainer);
        }

        public static async Task<string>
            UploadAvatarToStorage(Stream fileStream, string fileName,
                AzureStorageConfig _storageConfig)
        {
            fileName = UnitCodeToAsciiString(fileName);
            //  fileName = Guid.NewGuid().ToString() + fileName;
            string url = "https://" +
                         _storageConfig.AccountName +
                         ".blob.core.windows.net/" +
                         _storageConfig.UserContainer + "/" + fileName;

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

        public static async Task<long> GetFileContentLengthAsync(string fileUrl, IHttpClientFactory httpClient)
        {
            long contentLength = 0; //Unit: byte
            try
            {
                var client = httpClient.CreateClient();
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Head, fileUrl);
                var response = await client.SendAsync(httpRequestMessage);
                if (response.Content.Headers.TryGetValues("Content-Length", out var values))
                {
                    if (values?.Any() == true)
                    {
                        contentLength = long.Parse(values.First());
                    }
                }
            }
            catch 
            {
                //Ignore when can't get file content length 
            }


            return contentLength;
        }

        public static async Task<string> CopyFileToContainerAsync(string fileName, string id, string folderName,
            AzureStorageConfig storageConfig, string containerName)
        {
            fileName = UnitCodeToAsciiString(fileName);
            var stringUls = fileName.Split('/');

            var urlImage = $"https://{storageConfig.AccountName}.blob.core.windows.net" +
                           $"/{containerName}" +
                           $"/{id}" +
                           $"/{folderName}" +
                           $"/{HttpUtility.UrlEncode(stringUls[^1].Replace(" ", string.Empty))}";

            var blobUri = new Uri(urlImage);
            var blobUriTemp = new Uri(fileName);

            var storageCredentials =
                new StorageSharedKeyCredential(storageConfig.AccountName, storageConfig.AccountKey);

            var blobClient = new BlobClient(blobUri, storageCredentials);
            await blobClient.StartCopyFromUriAsync(blobUriTemp);

            return await Task.FromResult(urlImage);
        }

        public static async Task<bool> DeleteBlobInContainerByNameAsync(AzureStorageConfig storageConfig,
            List<string> urlList, string id, string containerName)
        {
            var accountUri = new Uri("https://" + storageConfig.AccountName + ".blob.core.windows.net/");

            var fileUrls = new List<string>();

            foreach (var url in urlList)
            {
                var urls = url.Split('/');
                var fileUrl = urls[^3] + "/" + urls[^2] + "/" + urls[^1];
                fileUrls.Add(fileUrl);
            }

            var storageCredentials =
                new StorageSharedKeyCredential(storageConfig.AccountName, storageConfig.AccountKey);
            var blobServiceClient = new BlobServiceClient(accountUri, storageCredentials);

            var container = blobServiceClient.GetBlobContainerClient(containerName);

            if (container.Exists())
            {
                var blob = container.GetBlobs(prefix: id);
                foreach (var blobItem in blob)
                {
                    if (fileUrls.Contains(blobItem.Name))
                    {
                        await container.DeleteBlobIfExistsAsync(blobItem.Name); 
                    }
                }
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Use this function to convert unicode string to ascii string -> Azure blob only accept string with ascii characters
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string UnitCodeToAsciiString(string inputString)
        {
            return Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                    ),
                    Encoding.UTF8.GetBytes(inputString)
                )
            );
        }

        public static async Task<bool> DeleteBlobsInLogsContainerAsync(AzureStorageConfig _storageConfig, int numberOfStorageDay)
        {
            // Create a URI to the storage account
            Uri accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            // Create BlobServiceClient from the account URI

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri, storageCredentials);

            // Get reference to the container
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(_storageConfig.LogsContainer);

            //  blobClient.Delete()
            if (container.Exists())
            {
                var prefix = DateTime.Now.AddDays(-numberOfStorageDay).ToString("yyyy/MM");
                var blobs = container.GetBlobs(prefix: prefix);
                foreach (var blobItem in blobs)
                {
                    await container.DeleteBlobIfExistsAsync(blobItem.Name);
                }
            }

            return await Task.FromResult(true);
        }

    }
}