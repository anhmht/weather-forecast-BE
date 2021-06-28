using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class HydrologicalForeCast : AuditableEntity //DbThuyVan
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StationId { get; set; }
        [Required]
        public DateTime RefDate { get; set; }
        public float? MinValue { get; set; }
        public float? MaxValue { get; set; }
        public int Type { get; set; } //Preventive

    }
}