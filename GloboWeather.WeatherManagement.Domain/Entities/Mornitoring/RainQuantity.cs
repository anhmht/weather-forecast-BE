using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class RainQuantity : AuditableEntity //LuongMua
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StationId { get; set; }
        [Required]
        public DateTime RefDate { get; set; }
        public float? Value { get; set; }

    }
}