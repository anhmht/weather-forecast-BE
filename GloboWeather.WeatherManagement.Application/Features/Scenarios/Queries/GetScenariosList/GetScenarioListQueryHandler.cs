using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList
{
    public class GetScenarioListQueryHandler: IRequestHandler<GetScenariosListQuery, GetScenariosListResponse>
    {
        private readonly IScenarioRepository _scenarioRepository;

        public GetScenarioListQueryHandler(IScenarioRepository scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
        }
        public async Task<GetScenariosListResponse> Handle(GetScenariosListQuery request, CancellationToken cancellationToken)
        {
            return await _scenarioRepository.GetByPagedAsync(request, cancellationToken);
        }
    }
}