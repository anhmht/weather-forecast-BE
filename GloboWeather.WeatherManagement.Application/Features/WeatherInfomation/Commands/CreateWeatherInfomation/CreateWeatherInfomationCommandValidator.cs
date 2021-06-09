using FluentValidation;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInfomations.Commands.CreateWeatherInfomation
{
    public class CreateWeatherInfomationCommandValidator : AbstractValidator<CreateWeatherInfomationCommand>
    {
        private readonly IWeatherInfomationRepository _weatherInfomationRepository;

        public CreateWeatherInfomationCommandValidator(IWeatherInfomationRepository weatherInfomationRepository)
        {
            _weatherInfomationRepository = weatherInfomationRepository;

            RuleFor(p => p.PositionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.RefDate)
                .NotEmpty().WithMessage("{PropertyName} is not null")
                .NotNull();
        }


    }
}