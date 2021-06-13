using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IMeteorologicalService
    {
        Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query);
    }
}