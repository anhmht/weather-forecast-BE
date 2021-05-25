using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Media;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IImageService
    {
        
        Task<ImageResponse> UploadImageAsync(IFormFile file);
        Task<List<string>> GetAllImagesAsync();
    }
}