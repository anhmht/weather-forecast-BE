using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario
{
    public class UpdateScenarioCommandValidator : AbstractValidator<UpdateScenarioCommand>
    {
        public UpdateScenarioCommandValidator()
        {
            RuleFor(s => s.ScenarioName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}