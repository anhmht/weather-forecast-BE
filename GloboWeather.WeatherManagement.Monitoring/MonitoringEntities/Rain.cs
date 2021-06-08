using System;

namespace GloboWeather.WeatherManagement.Monitoring.MonitoringEntities
{
    public class Rain
    {
        public string StationId { get; set; }
        public DateTime Date { get; set; }
        public float Quality { get; set; }
    }
}