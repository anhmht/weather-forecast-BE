using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IVideoService
    {
        Task RunAsync(IFormFile file);
    }
}