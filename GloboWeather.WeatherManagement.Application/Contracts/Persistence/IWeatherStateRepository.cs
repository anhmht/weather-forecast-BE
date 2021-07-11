using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IWeatherStateRepository : IAsyncRepository<WeatherState>
    {
        Task<GetWeatherStateListResponse> GetByPageAsync(GetWeatherStateListQuery query, CancellationToken token);
    }
}