using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario
{
    public class DeleteScenarioCommandHandler : IRequestHandler<DeleteScenarioCommand>
    {
        private readonly IScenarioRepository _scenarioRepository;

        public DeleteScenarioCommandHandler(IScenarioRepository scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
        }
        
        public async Task<Unit> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenarioToDelete = await _scenarioRepository.GetByIdAsync(request.ScenarioId);

            if (scenarioToDelete == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            await _scenarioRepository.DeleteAsync(scenarioToDelete);
            
            return  Unit.Value;
            
        }
    }
}