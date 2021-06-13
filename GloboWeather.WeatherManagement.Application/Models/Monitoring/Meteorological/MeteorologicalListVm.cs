using System;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical
{
    public class MeteorologicalListVm
    {
        public string ProvinceName { get; set; }
        public string StationName { get; set; }
        public DateTime Date { get; set; }
        public int ZipCode { get; set; }
        public string StationId { get; set; }
        public float? Evaporation { get; set; }
        public float? Radiation { get; set; }
        public float? Humidity { get; set; }
        public float? WindDirection { get; set; }
        public float? Barometric { get; set; }
        public float? Hga10 { get; set; }
        public float? Hgm60 { get; set; }
        public float? Rain { get; set; }
        public float? Temperature { get; set; }
        public float? Tdga10 { get; set; }
        public float? Tdgm60 { get; set; }
        public float? WindSpeed { get; set; }
        public float? ZluyKe { get; set; }
        public string Address { get; set; }
    }
}