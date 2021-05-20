using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class SitePage : AuditableEntity
    {
        public Guid SitePageId { get; set; }
        [MaxLength(60)]
        [Required]
        public string Title { get; set; }
        
        public  string MainContent { get; set; }
        
        public bool AllowDelete { get; set; }
        
        public  bool IsIndex { get; set; }
    }
}