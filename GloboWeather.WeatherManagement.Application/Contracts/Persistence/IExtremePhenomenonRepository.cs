using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IExtremePhenomenonRepository : IAsyncRepository<ExtremePhenomenon>
    {
        Task<GetExtremePhenomenonListResponse> GetByPageAsync(GetExtremePhenomenonListQuery query,
            CancellationToken token);
    }
}