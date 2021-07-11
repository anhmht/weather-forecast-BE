using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState
{
    public class CreateWeatherStateCommandValidator : AbstractValidator<CreateWeatherStateCommand>
    {
        
        public CreateWeatherStateCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

        }

        
    }
}