
using GloboWeather.WeatherManagement.Domain.Entities;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IBackgroundServiceTrackingRepository : IAsyncRepository<BackgroundServiceTracking>
    {

        Task<BackgroundServiceTracking> GetLastBackgroundServiceTracking();
    }
}