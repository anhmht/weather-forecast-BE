using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail
{
    public class GetScenarioDetailQueryHandler : IRequestHandler<GetScenarioDetailQuery, ScenarioDetailVm>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IMapper _mapper;

        public GetScenarioDetailQueryHandler(IScenarioRepository scenarioRepository,IMapper mapper)
        {
            _scenarioRepository = scenarioRepository;
            _mapper = mapper;
        }
        public async Task<ScenarioDetailVm> Handle(GetScenarioDetailQuery request, CancellationToken cancellationToken)
        {
            var @scenario = await _scenarioRepository.GetByIdAsync(request.ScenarioId);
            if (@scenario == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            var scenarioDto = _mapper.Map<ScenarioDetailVm>(@scenario);
            return scenarioDto;
        }
    }
}