using FluentValidation;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

namespace GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import
{
    public class ImportRainQuantityDtoValidator : AbstractValidator<ImportRainQuantityDto>
    {
        public ImportRainQuantityDtoValidator()
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
            RuleFor(x => x.Value)
                .Custom(ValidateHelper.IsNumber());
        }

    }
}
