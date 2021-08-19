using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval
{
    public class GetCommentDetailForApprovalValidator : AbstractValidator<GetCommentDetailForApprovalQuery>
    {
        public GetCommentDetailForApprovalValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
