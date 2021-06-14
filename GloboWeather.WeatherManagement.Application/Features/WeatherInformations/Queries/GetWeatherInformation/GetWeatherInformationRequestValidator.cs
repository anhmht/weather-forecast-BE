using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    class GetWeatherInformationRequestValidator : AbstractValidator<GetWeatherInformationRequest>
    {
        public GetWeatherInformationRequestValidator()
        {
            //RuleFor(p => p.FromDate)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(p => p.ToDate)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(p => p.ToDate)
            //    .GreaterThanOrEqualTo(p => p.FromDate).WithMessage("ToDate must be greater or equal to FromDate.")
            //    .NotNull();
        }
    }
}
