using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public class GetEventsListWithContentQueryValidator : AbstractValidator<GetEventsListWithContentQuery>
    {
        public GetEventsListWithContentQueryValidator()
        {
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            
            RuleFor(p => p.StatusId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

        }
        
        
      
    }
}