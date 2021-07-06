using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationResponse: BaseResponse
    {
        public IEnumerable<RowError> RowErrors { get; set; }
    }
}