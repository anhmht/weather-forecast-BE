
using FluentValidation;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IEventRepository _eventRepository;

        public CreateEventCommandValidator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ImageUrl)
                .NotEmpty().WithMessage("{PropertyName} is not null")
                .NotNull();
        }

        
    }
}