using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction
{
    public class CreateScenarioActionCommandHandler : IRequestHandler<CreateScenarioActionCommand, Guid>
    {
        private readonly IScenarioService _scenarioService;

        public CreateScenarioActionCommandHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }
        public async Task<Guid> Handle(CreateScenarioActionCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateScenarioActionCommandValidator();

            var validationResult = validator.Validate(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _scenarioService.CreateAsync(request, cancellationToken);

        }
    }
}