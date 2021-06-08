using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class GetRainMinMaxResponse
    {
        public string ProviceName { get; set; }
        public DateTime Date { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
    }
}