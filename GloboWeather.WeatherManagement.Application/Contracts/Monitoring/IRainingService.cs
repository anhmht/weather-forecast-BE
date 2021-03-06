using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IRainingService
    {
        Task<GetRainListResponse> GetByPagedAsync(GetRainsListQuery query);
    }
}