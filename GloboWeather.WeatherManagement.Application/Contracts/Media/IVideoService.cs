using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IVideoService
    {
        Task<IList<string>> UploadVideoAsync(IFormFile file);

        Task<IList<string>> UploadVideoSocialAsync(IFormFile file);
    }
}