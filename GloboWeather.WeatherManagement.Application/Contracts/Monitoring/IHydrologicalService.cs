using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IHydrologicalService
    {
        Task<GetHydrologicalListResponse> GetByPagedAsync(GetHydrologicalListQuery query);
    }
}