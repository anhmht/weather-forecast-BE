using System;
using System.Collections.Generic;
using System.IO;
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
        public  AzureStorageConfig _storageConfig;
        private readonly ILogger<ImageService> _logger;

        public  ImageService(IOptions<AzureStorageConfig> azureStorageConfig, ILogger<ImageService> logger)
        {
            _storageConfig = azureStorageConfig.Value;
            _logger = logger;
        }

        public async Task<ImageResponse> UploadImageAsync(IFormFile file)
        {
            string imageUrl = string.Empty;
            if (StorageHelper.IsImage(file))
            {
                if (file.Length > 0)
                {
                    using (Stream stream = file.OpenReadStream())
                    {
                        _logger.LogInformation("Image Upload");
                        var fileName = Guid.NewGuid().ToString() + file.FileName;
                        imageUrl = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
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

        public async Task<bool> DeleteAllImagesAsync()
        {
            return await StorageHelper.DeleteBlobInTempsAsync(_storageConfig);
        }
    }
}