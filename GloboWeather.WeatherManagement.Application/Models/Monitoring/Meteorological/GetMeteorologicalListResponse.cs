using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class GetMeteorologicalListResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<MeteorologicalListVm> Meteorologicals { get; init; }
    }
}