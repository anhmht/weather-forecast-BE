using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IHydrologicalForecastService
    {
        Task<GetHydrologicalForecastListResponse> GetByPagedAsync(GetHydrologicalForecastListQuery query);
    }
}