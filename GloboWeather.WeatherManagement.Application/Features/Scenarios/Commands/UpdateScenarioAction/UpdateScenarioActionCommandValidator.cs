using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction
{
    public class UpdateScenarioActionCommandValidator : AbstractValidator<UpdateScenarioActionCommand>
    {
        public UpdateScenarioActionCommandValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}