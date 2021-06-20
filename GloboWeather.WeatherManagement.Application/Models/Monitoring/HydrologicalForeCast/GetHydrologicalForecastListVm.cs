using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast
{
    public class HydrologicalForecastListVm
    {
        public string StationId { get; set; }
        public DateTime RefDate { get; set; }
        public List<HydrologicalForecastVm> Data { get; set; }

        public HydrologicalForecastListVm()
        {
            Data = new List<HydrologicalForecastVm>();
        }
    }

    public class HydrologicalForecastVm
    {
        public DateTime Date { get; set; }
        public float? Min { get; set; }
        public float? Max { get; set; }
        public string Value { get; set; }
    }
}
