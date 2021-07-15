using System.Globalization;
using FluentValidation;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.UpdateExtremePhenomenon
{
    public class UpdateExtremePhenomenonCommandValidator : AbstractValidator<UpdateExtremePhenomenonCommand>
    {
        
        public UpdateExtremePhenomenonCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.DistrictId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.ProvinceId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Date)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Date.ToString(CultureInfo.InvariantCulture))
                .Must(ValidateHelper.BeAValidDate).WithMessage("{PropertyName} is invalid datetime.")
                .NotNull();
            RuleForEach(x => x.Details)
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .NotNull().WithMessage("{PropertyName} is required.");
        }

        
    }
}