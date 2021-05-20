using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Theme : AuditableEntity
    {
        public Guid ThemeId { get; set; }
        public  bool IsSelected { get; set; }
        public  string TextDomain { get; set; }
    }
}