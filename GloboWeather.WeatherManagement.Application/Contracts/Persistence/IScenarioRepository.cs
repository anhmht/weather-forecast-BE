using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IScenarioRepository : IAsyncRepository<Scenario>
    {
        Task<GetScenariosListResponse> GetByPagedAsync(GetScenariosListQuery query, CancellationToken token);
        Task<Scenario> AddAsync(Scenario entity);
        Task<int> UpdateAsync(Scenario entity);
        Task<int> DeleteAsync(Scenario entity);
    }
}