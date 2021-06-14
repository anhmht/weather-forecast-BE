using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Scenario : AuditableEntity
    {
        
        [Required]
        public Guid ScenarioId { get; set; }
        
        [Required]
        public string ScenarioName { get; set; }
        
        [Required]
        public string ScenarioContent { get; set; } 
    }
}