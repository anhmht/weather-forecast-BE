using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction
{
    public class UpdateScenarioActionCommandHandler : IRequestHandler<UpdateScenarioActionCommand, Guid>
    {
        private readonly IScenarioService _scenarioService;

        public UpdateScenarioActionCommandHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }
        public async Task<Guid> Handle(UpdateScenarioActionCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateScenarioActionCommandValidator();

            var validationResult = validator.Validate(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _scenarioService.UpdateScenarioActionAsync(request, cancellationToken);

        }
    }
}