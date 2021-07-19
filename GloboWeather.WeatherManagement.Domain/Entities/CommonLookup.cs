using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class CommonLookup : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string NameSpace { get; set; }
        public int ValueId { get; set; }
        public string ValueText { get; set; } 
        public string Description { get; set; }
    }
}
