using GloboWeather.WeatherManagement.Application.Models.Weather;
using MediatR;
using System;

namespace GloboWeather.WeatherManagement.Application.Requests
{
    public class WeatherInformationBaseRequest
    {
        public string StationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
