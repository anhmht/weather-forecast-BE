
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IMeteorologicalRepository : IAsyncRepository<Meteorological>
    {
        Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query);
        Task<List<Meteorological>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }
}