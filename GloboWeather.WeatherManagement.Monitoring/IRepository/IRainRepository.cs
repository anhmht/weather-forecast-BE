using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.IRepository
{
    public interface IRainRepository : IAsyncRepository<Rain>
    {
         Task<List<GetRainResponse>> GetRainQuantityAsync(IEnumerable<int> zipcodes);
    }
}