using System;

namespace GloboWeather.WeatherManagement.Monitoring.MonitoringEntities
{
    public class HydrologicalForecast
    {
        public string StationId { get; set; }
        public string MinMax { get; set; }
        public DateTime RefDate { get; set; }
        public float? Day1 { get; set; }
        public float? Day2 { get; set; }
        public float? Day3 { get; set; }
        public float? Day4 { get; set; }
        public float? Day5 { get; set; }
    }
}
