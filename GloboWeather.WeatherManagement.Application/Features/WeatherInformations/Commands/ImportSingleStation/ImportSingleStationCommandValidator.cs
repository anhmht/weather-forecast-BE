using System;
using FluentValidation;
using FluentValidation.Validators;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation
{
    public class ImportSingleStationCommandValidator : AbstractValidator<ImportSingleStationCommand>
    {
        private readonly IWeatherInformationRepository _weatherInformationRepository;

        public ImportSingleStationCommandValidator(IWeatherInformationRepository weatherInformationRepository)
        {
            _weatherInformationRepository = weatherInformationRepository;

            RuleFor(p => p.StationId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.StationName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.File)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}