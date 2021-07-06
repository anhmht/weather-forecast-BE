using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IMeteorologicalRepository : IAsyncRepository<Meteorological>
    {
        Task<int> DownloadDataAsync(List<Meteorological> meteorologicalForeCasts, DownloadDataRequest request);
        Task<int> ImportAsync(ImportMeteorologicalCommand request, List<Meteorological> meteorologicals, CancellationToken cancellationToken);
    }
}