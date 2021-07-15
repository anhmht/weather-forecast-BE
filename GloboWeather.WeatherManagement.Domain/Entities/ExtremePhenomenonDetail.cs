using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ExtremePhenomenonDetail : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid ExtremePhenomenonId { get; set; }
        
        public string Name { get; set; }
        
        public string Content  { get; set; }
        
    }
}