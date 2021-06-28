using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Hydrological : AuditableEntity //ThuyVan
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string StationId { get; set; }
        [Required]
        public DateTime RefDate { get; set; }
        public float? Rain { get; set; }
        public float? WaterLevel { get; set; }
        public float? Accumulated { get; set; } //ZLuyKe

    }
}