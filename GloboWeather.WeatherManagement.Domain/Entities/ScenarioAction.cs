using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ScenarioAction : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ScenarioId { get; set; }
        public int? ActionTypeId { get; set; }
        public int? MethodId { get; set; }
        public int? AreaTypeId { get; set; }
        public string Data { get; set; }
        public int? Duration { get; set; }
        public int Order { get; set; }
        public float? Top { get; set; }
        public float? Left { get; set; }
        public float? Bottom { get; set; }
        public float? Right { get; set; }
        public bool? IsEnableIcon { get; set; }
        public bool? IsEnableLayer { get; set; }
        public bool? IsDisplayHydrological { get; set; }
    }
}
