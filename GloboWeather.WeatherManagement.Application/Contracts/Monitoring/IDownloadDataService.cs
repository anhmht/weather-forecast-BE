using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Requests;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IDownloadDataService
    {
        Task DownloadDataAsync(DownloadDataRequest request);
    }
}