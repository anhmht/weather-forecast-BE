using FluentValidation;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Helpers.Validator;

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
                .Must(ValidateHelper.BeAValidDate).WithMessage("{PropertyName} is invalid datetime.")
                .NotNull();
            RuleFor(x => x.DoAm)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.GioGiat)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.LuongMua)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.NhietDo)
                .Custom(ValidateHelper.IsNumber());
            RuleFor(x => x.TocDoGio)
                .Custom(ValidateHelper.IsNumber());
        }


    }
}