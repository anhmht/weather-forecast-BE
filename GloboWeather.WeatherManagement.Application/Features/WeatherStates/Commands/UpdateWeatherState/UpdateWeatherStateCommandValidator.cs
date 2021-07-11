using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.UpdateWeatherState
{
    public class UpdateWeatherStateCommandValidator : AbstractValidator<UpdateWeatherStateCommand>
    {
        
        public UpdateWeatherStateCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }

        
    }
}