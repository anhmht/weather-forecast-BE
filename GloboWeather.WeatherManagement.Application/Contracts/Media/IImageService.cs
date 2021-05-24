using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<List<string>> GetAllImagesAsync();
    }
}