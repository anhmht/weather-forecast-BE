using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IMeteorologicalRepository : IAsyncRepository<Meteorological>
    {
        Task<int> DownloadDataAsync(List<Meteorological> meteorologicalForeCasts, DownloadDataRequest request);
    }
}