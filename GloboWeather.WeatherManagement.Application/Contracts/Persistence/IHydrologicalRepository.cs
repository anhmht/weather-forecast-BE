using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IHydrologicalRepository : IAsyncRepository<Hydrological>
    {
        Task<int> DownloadDataAsync(List<Hydrological> hydrologicalForeCasts, DownloadDataRequest request);
        Task<int> ImportAsync(ImportHydrologicalCommand request, List<Hydrological> hydrologicals, CancellationToken cancellationToken);
    }
}