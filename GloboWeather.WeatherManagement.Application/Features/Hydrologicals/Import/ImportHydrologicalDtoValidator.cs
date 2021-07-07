using FluentValidation;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

namespace GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import
{
    public class ImportHydrologicalDtoValidator : AbstractValidator<ImportHydrologicalDto>
    {
        public ImportHydrologicalDtoValidator()
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
            RuleFor(x => x.WaterLevel)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Accumulated)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.Rain)
                .Custom(ValidateHelper.IsNumber());
        }

    }
}
