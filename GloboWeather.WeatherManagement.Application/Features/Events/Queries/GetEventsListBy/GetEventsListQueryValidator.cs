using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
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