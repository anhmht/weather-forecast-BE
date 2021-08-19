using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval
{
    public class GetPostDetailForApprovalValidator : AbstractValidator<GetPostDetailForApprovalQuery>
    {
        public GetPostDetailForApprovalValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
