using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class HydrologicalListVm
    {
        public DateTime Date { get; set; }
        public float?  Rain { get; set; }
        public float? WaterLevel { get; set; }
        public float? ZLuyKe { get; set; }
       
    }
}