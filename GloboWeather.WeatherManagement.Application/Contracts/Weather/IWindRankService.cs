using System.Threading.Tasks;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWindRankService
    {
        Task<int> DownloadAsync();
    }
}