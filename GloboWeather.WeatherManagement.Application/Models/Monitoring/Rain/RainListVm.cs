using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class RainListVm
    {
        
        public string ProvinceName { get; set; }
        public string StationName { get; set; }
        public DateTime Date { get; set; }
        public int ZipCode { get; set; }
        public string StationId { get; set; }
        public float RainQuantity { get; set; }
        public string Address { get; set; }
    }
}