using FluentValidation;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon
{
    public class AddActionIconCommandValidator : AbstractValidator<AddActionIconCommand>
    {
        public AddActionIconCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.IconId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
