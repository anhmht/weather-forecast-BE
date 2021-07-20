using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario
{
    public class CreateScenarioCommandValidator : AbstractValidator<CreateScenarioCommand>
    {
        public CreateScenarioCommandValidator()
        {
            RuleFor(s => s.ScenarioName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}