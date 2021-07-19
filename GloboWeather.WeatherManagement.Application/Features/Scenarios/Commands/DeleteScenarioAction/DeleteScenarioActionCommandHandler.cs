using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction
{
    public class DeleteScenarioActionCommandHandler : IRequestHandler<DeleteScenarioActionCommand>
    {
        private readonly IScenarioService _scenarioService;

        public DeleteScenarioActionCommandHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        public async Task<Unit> Handle(DeleteScenarioActionCommand request, CancellationToken cancellationToken)
        {
            await _scenarioService.DeleteAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}