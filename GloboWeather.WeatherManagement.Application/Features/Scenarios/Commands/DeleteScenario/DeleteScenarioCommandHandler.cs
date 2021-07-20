using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario
{
    public class DeleteScenarioCommandHandler : IRequestHandler<DeleteScenarioCommand>
    {
        private readonly IScenarioService _scenarioService;

        public DeleteScenarioCommandHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }
        
        public async Task<Unit> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
        {
            await _scenarioService.DeleteScenarioAsync(request, cancellationToken);
            return Unit.Value;

        }
    }
}