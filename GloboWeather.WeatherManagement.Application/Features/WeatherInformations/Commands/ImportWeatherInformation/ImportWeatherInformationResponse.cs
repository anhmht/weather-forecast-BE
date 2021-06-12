using FluentValidation.Results;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public IEnumerable<RowError> RowErrors { get; set; }
    }

    public class RowError
    {
        public int RowIndex { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
    }
}