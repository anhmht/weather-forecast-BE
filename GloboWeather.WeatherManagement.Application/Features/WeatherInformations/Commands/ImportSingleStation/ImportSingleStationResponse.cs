using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation
{
    public class ImportSingleStationResponse: BaseResponse
    {
        public GetWeatherInformationResponse Data { get; set; }
        public IEnumerable<RowError> RowErrors { get; set; }
    }
}