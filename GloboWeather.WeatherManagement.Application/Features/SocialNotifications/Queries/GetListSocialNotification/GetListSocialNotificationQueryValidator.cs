using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification
{
    public class GetListSocialNotificationQueryValidator : AbstractValidator<GetListSocialNotificationQuery>
    {
        public GetListSocialNotificationQueryValidator()
        {
            //RuleFor(p => p.Id)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();
        }
    }
}
