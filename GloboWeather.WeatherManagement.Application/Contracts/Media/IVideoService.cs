using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IVideoService
    {
        Task<IList<string>> RunAsync(IFormFile file);
    }
}