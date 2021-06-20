using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IHydrologicalForecastRepository : IAsyncRepository<HydrologicalForecast>
    {
        Task<GetHydrologicalForecastListResponse> GetByPagedAsync(GetHydrologicalForecastListQuery query);
    }
}