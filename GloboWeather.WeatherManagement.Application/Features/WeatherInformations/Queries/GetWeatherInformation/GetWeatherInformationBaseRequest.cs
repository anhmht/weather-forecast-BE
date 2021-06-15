using GloboWeather.WeatherManagement.Application.Helpers.Common;
using MediatR;
using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationBaseRequest
    {
        public IEnumerable<string> StationIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IEnumerable<WeatherType> WeatherTypes { get; set; }
    }
}
