using FluentValidation;
using GloboWeather.WeatherManagement.Application.Requests;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class WeatherInformationBaseRequestValidator : AbstractValidator<WeatherInformationBaseRequest>
    {
        public WeatherInformationBaseRequestValidator()
        {
            RuleFor(p => p.StationId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            
            RuleFor(p => p.FromDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ToDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

        }
        
        
      
    }
}