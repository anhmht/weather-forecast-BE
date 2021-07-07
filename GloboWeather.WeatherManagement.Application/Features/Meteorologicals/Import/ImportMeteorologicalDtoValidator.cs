using FluentValidation;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

namespace GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import
{
    public class ImportMeteorologicalDtoValidator : AbstractValidator<ImportMeteorologicalDto>
    {
        public ImportMeteorologicalDtoValidator()
        {

            RuleFor(p => p.StationId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Date)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Date)
                .Must(ValidateHelper.BeAValidDate).WithMessage("{PropertyName} is invalid datetime.")
                .NotNull();
            RuleFor(x => x.Evaporation)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Radiation)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Humidity)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.WindDirection)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Barometric)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Hga10)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Hgm60)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Rain)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Temperature)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Tdga10)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Tdgm60)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.WindSpeed)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.SunnyTime)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.ZluyKe)
                .Custom(ValidateHelper.IsNumber());
        }

    }
}
