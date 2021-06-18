using FluentValidation;
using System;
using FluentValidation.Validators;
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
            RuleFor(x => x.DoAm)
                .Custom(IsNumber());
            RuleFor(x => x.GioGiat)
                .Custom(IsNumber());
            RuleFor(x => x.LuongMua)
                .Custom(IsNumber());
            RuleFor(x => x.NhietDo)
                .Custom(IsNumber());
            RuleFor(x => x.TocDoGio)
                .Custom(IsNumber());
        }

        private static Action<string, CustomContext> IsNumber()
        {
            return (x, context) =>
            {
                if (!string.IsNullOrWhiteSpace(x) && !int.TryParse(x, out int value))
                {
                    context.AddFailure($"{x} is not a valid number.");
                }
            };
        }

        private bool BeAValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

    }
}