using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using GloboWeather.WeatherManagement.Application.Models.Media;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Infrastructure.Helpers;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.Media
{
    public class ImageService : IImageService
    {
        public AzureStorageConfig _storageConfig;
        private readonly ILogger<ImageService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public  ImageService(IOptions<AzureStorageConfig> azureStorageConfig, ILogger<ImageService> logger, IHttpClientFactory httpClientFactory)
        {
            _storageConfig = azureStorageConfig.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            
        }

        public async Task<ImageResponse> UploadImageAsync(IFormFile file)
        {
            if (!StorageHelper.IsImage(file))
                return new ImageResponse
                {
                    Url = string.Empty
                };

            string imageUrl = string.Empty;

            if (file.Length > 0)
            {
                using (Stream stream = file.OpenReadStream())
                {
                    _logger.LogInformation("File Upload");
                    var fileName = Guid.NewGuid().ToString() + file.FileName.Replace(" ", String.Empty);
                    imageUrl = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                _logger.LogError("File uploading failed");
            }

            return new ImageResponse
            {
                Url = imageUrl
            };
        }

        public async Task<List<string>> CopyImageToEventPost(List<string> imageUrls, string eventId, string folderName)
        {
            List<string> imageUrlsAfterPost = new List<string>();
            if (imageUrls.Any() == false)
            {
                return null;
            }

            foreach (var imageUrl in imageUrls)
            {
                imageUrlsAfterPost.Add(await StorageHelper.CopyFileToContainerPost(imageUrl, eventId, folderName,_storageConfig)); 
            }

            return imageUrlsAfterPost ;
        }

        public async Task<ImageResponse> UploadAvatarForUserAsync(string userId, IFormFile file)
        {
            string imageUrl = string.Empty;
            if (StorageHelper.IsImage(file))
            {
                if (file.Length > 0)
                {
                    using (Stream stream = file.OpenReadStream())
                    {
                        _logger.LogInformation("Image Upload");
                        var fileName = userId + file.FileName.Replace(" ", String.Empty);
                        imageUrl = await StorageHelper.UploadAvartarToStorage(stream, fileName, _storageConfig);
                    }
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                _logger.LogError("Image uploading failed");
            }

            return new ImageResponse
            {
                Url = imageUrl
            };
        }

        public async Task<List<string>> GetAllImagesAsync()
        {
            return await StorageHelper.GetImageUrls(_storageConfig);
        }

        public async Task<bool> DeleteAllImagesTempContainerAsync()
        {
            return await StorageHelper.DeleteBlobsInTempContainerAsync(_storageConfig);
        }

        public async Task<bool> DeleteImagesInPostsContainerAsync(string eventId)
        {
            return await StorageHelper.DeleteBlobInPostContainerAsync(_storageConfig, eventId);
        }
        
        public async Task<bool> DeleteImagesInPostsContainerByNameAsync(string eventId, List<string> imageUrls)
        {
            return await StorageHelper.DeleteBlobInPostContainerByNameAsync(_storageConfig,imageUrls, eventId);
        }

        public async Task<ImageResponse> GenerateQRCodeAsync(string text)
        {
            var qrCode = QRCodeHelper.CreateQRCodeStream(text);
            
            var fileName = $"QR_CODE_{Guid.NewGuid()}.jpg";
            var url = await StorageHelper.UploadFileToStorage(qrCode.FileStream, fileName, _storageConfig);
            await qrCode.FileStream.DisposeAsync();
            File.Delete(qrCode.FilePath);
            return new ImageResponse
            {
                Url = url
            };
        }

        public async Task<DocumentResponse> UploadFileAsync(IFormFile file)
        {
            string imageUrl = string.Empty;

            if (file.Length > 0)
            {
                using (Stream stream = file.OpenReadStream())
                {
                    _logger.LogInformation("File Upload");
                    var fileName = Guid.NewGuid().ToString() + file.FileName.Replace(" ", String.Empty);
                    imageUrl = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                _logger.LogError("File uploading failed");
            }

            return new DocumentResponse
            {
                Url = imageUrl,
                ContentLength = file.Length
                
            };
        }

        public async Task<List<string>> CopyFileToStorageContainerAsync(List<string> files, string id, string folderName, string containerName)
        {
            var result = new List<string>();
            if (files.Any() == false)
            {
                return null;
            }

            foreach (var file in files)
            {
                result.Add(await StorageHelper.CopyFileToContainerAsync(file, id, folderName, _storageConfig, containerName));
            }

            return result;
        }

        public async Task<bool> DeleteFileInStorageContainerByNameAsync(string id, List<string> files, string containerName)
        {
            return await StorageHelper.DeleteBlobInContainerByNameAsync(_storageConfig, files, id, containerName);
        }
    }
}