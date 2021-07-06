using System;
using FluentValidation.Validators;

namespace GloboWeather.WeatherManagement.Application.Helpers.Validator
{
    public static class ValidateHelper
    {

        public static Action<string, CustomContext> IsNumber()
        {
            return (x, context) =>
            {
                if (!string.IsNullOrWhiteSpace(x) && !float.TryParse(x, out _))
                {
                    context.AddFailure($"{x} is not a valid number.");
                }
            };
        }

        public static bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out _);
        }
    }
}
