using FluentValidation;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation
{
    public class CreateWeatherInformationCommandValidator : AbstractValidator<CreateWeatherInformationCommand>
    {
    

        public CreateWeatherInformationCommandValidator()
        {          

            //RuleFor(p => p.StationId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(p => p.RefDate)
            //    .NotEmpty().WithMessage("{PropertyName} is not null")
            //    .NotNull();
        }


    }
}