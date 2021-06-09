using FluentValidation;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation
{
    public class CreateWeatherInformationCommandValidator : AbstractValidator<CreateWeatherInfofmationCommand>
    {
        private readonly IWeatherInformationRepository _WeatherInformationRepository;

        public CreateWeatherInformationCommandValidator(IWeatherInformationRepository WeatherInformationRepository)
        {
            _WeatherInformationRepository = WeatherInformationRepository;

            RuleFor(p => p.PositionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.RefDate)
                .NotEmpty().WithMessage("{PropertyName} is not null")
                .NotNull();
        }


    }
}