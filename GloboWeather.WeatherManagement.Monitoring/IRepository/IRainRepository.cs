using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IRainRepository : IAsyncRepository<Rain>
    {
        Task<GetRainListResponse> GetByPagedAsync(GetRainsListQuery query);
        Task<List<Rain>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }
}