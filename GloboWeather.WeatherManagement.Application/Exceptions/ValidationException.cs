using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace GloboWeather.WeatherManagement.Application.Exceptions
{
    public class ValidationException: ApplicationException
    {
        public List<string> ValidationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();
            foreach (var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }

    }
}