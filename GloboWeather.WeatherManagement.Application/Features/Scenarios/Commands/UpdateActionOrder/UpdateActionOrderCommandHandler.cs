using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateActionOrder
{
    class UpdateActionOrderCommandHandler : IRequestHandler<UpdateActionOrderCommand>
    {
        private readonly IScenarioService _scenarioService;
        public UpdateActionOrderCommandHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        public async Task<Unit> Handle(UpdateActionOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateActionOrderValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            await _scenarioService.UpdateActionOrderAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}
