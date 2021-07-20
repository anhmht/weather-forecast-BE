using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail
{
    public class GetScenarioActionDetailQueryHandler : IRequestHandler<GetScenarioActionDetailQuery, ScenarioActionDetailVm>
    {
        private readonly IScenarioService _scenarioService;

        public GetScenarioActionDetailQueryHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        public async Task<ScenarioActionDetailVm> Handle(GetScenarioActionDetailQuery request, CancellationToken cancellationToken)
        {
            return await _scenarioService.GetScenarioActionDetailAsync(request);
        }
    }
}