using FluentValidation;
using System;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationValidator : AbstractValidator<ImportWeatherInformationDto>
    {
        private readonly IWeatherInformationRepository _weatherInformationRepository;

        public ImportWeatherInformationValidator(IWeatherInformationRepository weatherInformationRepository)
        {
            _weatherInformationRepository = weatherInformationRepository;

            RuleFor(p => p.DiaDiemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.NgayGio)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.NgayGio)
                .Must(BeAValidDate).WithMessage("{PropertyName} is invalid datetime.")
                .NotNull();
        }
        private bool BeAValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

    }
}