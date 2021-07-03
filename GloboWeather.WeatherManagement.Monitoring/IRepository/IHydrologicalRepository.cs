using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IHydrologicalRepository : IAsyncRepository<Hydrological>
    {
        Task<GetHydrologicalListResponse> GetByPagedAsync(GetHydrologicalListQuery query);
        Task<List<Hydrological>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }
}