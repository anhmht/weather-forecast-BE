using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Commands.UpdateIsReadSocialNotification
{
    public class UpdateIsReadSocialNotificationCommandValidator : AbstractValidator<UpdateIsReadSocialNotificationCommand>
    {
        public UpdateIsReadSocialNotificationCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
