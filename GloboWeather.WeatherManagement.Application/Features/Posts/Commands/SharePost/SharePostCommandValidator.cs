using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.SharePost
{
    public class SharePostCommandValidator : AbstractValidator<SharePostCommand>
    {
        public SharePostCommandValidator()
        {
            RuleFor(p => p.PostId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
