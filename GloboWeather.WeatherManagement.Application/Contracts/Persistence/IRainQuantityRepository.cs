using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IRainQuantityRepository : IAsyncRepository<RainQuantity>
    {
        Task<int> DownloadDataAsync(List<RainQuantity> rainQuantityForeCasts, DownloadDataRequest request);
        Task<int> ImportAsync(ImportRainQuantityCommand request, List<RainQuantity> rainQuantities, CancellationToken cancellationToken);
    }
}