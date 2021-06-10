
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IMeteorologicalRepository : IAsyncRepository<Meteorological>
    {
        Task<List<GetMeteorologicalResponse>> GetMeteorologicalsAsync(IEnumerable<int> zipcodes);
    }
}