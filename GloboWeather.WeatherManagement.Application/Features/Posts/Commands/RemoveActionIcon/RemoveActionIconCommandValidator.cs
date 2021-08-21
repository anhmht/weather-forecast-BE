using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.RemoveActionIcon
{
    public class RemoveActionIconCommandValidator : AbstractValidator<RemoveActionIconCommand>
    {
        public RemoveActionIconCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
