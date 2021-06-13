using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class HydrologicalListVm
    {
        public string ProvinceName { get; set; }
        public int ZipCode { get; set; }
        public string StationName { get; set; }
        public string StationId { get;set; }
        public DateTime Date { get; set; }
        public float  Rain { get; set; }
        public float WaterLevel { get; set; }
        public float ZLuyKe { get; set; }
        public string Address { get; set; }
    }
}