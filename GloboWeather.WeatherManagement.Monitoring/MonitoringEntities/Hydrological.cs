using System;

namespace GloboWeather.WeatherManagement.Monitoring.MonitoringEntities
{
    public class Hydrological
    {
        public string StationId { get; set; }
        public DateTime Date { get; set; }
        public float?  Rain { get; set; }
        public float? WaterLevel { get; set; }
        public float? ZLuyKe { get; set; }
    }
}