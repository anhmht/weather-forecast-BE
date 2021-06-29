using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Meteorological : AuditableEntity //KhiTuong
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StationId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public float? Evaporation { get; set; } //bochoi
        public float? Radiation { get; set; } //bucxa
        public float? Humidity { get; set; } //doam
        public float? WindDirection { get; set; } //huondgio
        public float? Barometric { get; set; } //khiap
        public float? Hga10 { get; set; }
        public float? Hgm60 { get; set; }
        public float? Rain { get; set; } //mua
        public float? Temperature { get; set; } //nhietdo
        public float? Tdga10 { get; set; }
        public float? Tdgm60 { get; set; }
        public float? WindSpeed { get; set; } //tocdogio
        public float? SunnyTime { get; set; } //thoignang
        public float? ZluyKe { get; set; }

    }
}