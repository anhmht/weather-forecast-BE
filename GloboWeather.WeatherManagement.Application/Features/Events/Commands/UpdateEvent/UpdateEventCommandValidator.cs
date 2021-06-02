using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator: AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotEmpty();

        }        
    }
}