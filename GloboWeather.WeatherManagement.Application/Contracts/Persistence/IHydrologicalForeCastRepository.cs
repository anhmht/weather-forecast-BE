using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IHydrologicalForeCastRepository : IAsyncRepository<HydrologicalForeCast>
    {
        Task<int> DownloadDataAsync(List<HydrologicalForeCast> hydrologicalForeCasts, DownloadDataRequest request);
        Task<int> ImportAsync(ImportHydrologicalForeCastCommand request, List<HydrologicalForeCast> hydrologicals, CancellationToken cancellationToken);
    }
}