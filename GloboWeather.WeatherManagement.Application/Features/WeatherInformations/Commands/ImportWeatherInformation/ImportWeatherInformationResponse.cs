using FluentValidation.Results;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManegement.Application.Responses;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationResponse: BaseResponse
    {
        public IEnumerable<RowError> RowErrors { get; set; }
    }

    public class RowError
    {
        public int RowIndex { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
    }
}