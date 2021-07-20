using System.Linq;
using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction
{
    public class CreateScenarioActionCommandValidator : AbstractValidator<CreateScenarioActionCommand>
    {
        public CreateScenarioActionCommandValidator()
        {
            RuleFor(s => s.ScenarioId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}