using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Theme : AuditableEntity
    {
        public Guid ThemeId { get; set; }
        
        public  bool IsSelected { get; set; }
        
        [Required]
        [MaxLength(50)]
        public  string TextDomain { get; set; }
    }
}