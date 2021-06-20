using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast
{
    public class GetHydrologicalForecastListQuery
    {
        public int Limit { get; set; }
        public int Page { get; set; }

        public string[] StationIds { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
