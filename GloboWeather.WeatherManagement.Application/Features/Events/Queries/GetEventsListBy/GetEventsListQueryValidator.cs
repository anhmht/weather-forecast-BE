using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy
{
    public class GetEventsListQueryValidator : AbstractValidator<GetEventsListByQuery>
    {
        public GetEventsListQueryValidator()
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