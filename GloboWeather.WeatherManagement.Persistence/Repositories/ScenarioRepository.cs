using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ScenarioRepository : BaseRepository<Scenario>, IScenarioRepository
    {
        public ScenarioRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<GetScenariosListResponse> GetByPagedAsync(GetScenariosListQuery query, CancellationToken token)
        {
            var scenarios = await _dbContext.Scenarios
                .AsNoTracking()
                .PaginateAsync(query.Page, query.Limit, token);

            return new GetScenariosListResponse
            {
                CurrentPage = scenarios.CurrentPage,
                TotalPages = scenarios.TotalPages,
                TotalItems = scenarios.TotalItems,
                Scenarios = scenarios.Items.Select(s => new ScenariosListVm()
                {
                    ScenarioId = s.ScenarioId,
                    ScenarioContent = s.ScenarioContent,
                    ScenarioName = s.ScenarioName
                }).ToList()
            };
            
            
        }
    }
}