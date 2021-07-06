using FluentValidation;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

namespace GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import
{
    public class ImportHydrologicalForeCastDtoValidator : AbstractValidator<ImportHydrologicalForeCastDto>
    {
        public ImportHydrologicalForeCastDtoValidator()
        {

            RuleFor(p => p.StationId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.RefDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.RefDate)
                .Must(ValidateHelper.BeAValidDate).WithMessage("{PropertyName} is invalid datetime.")
                .NotNull();
            RuleFor(x => x.MinValue)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.MaxValue)
                .Custom(ValidateHelper.IsNumber());
        }

    }
}
