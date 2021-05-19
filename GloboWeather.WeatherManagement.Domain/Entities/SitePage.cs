using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class SitePage : AuditableEntity
    {
        public Guid SitePageId { get; set; }
        public string Title { get; set; }
        public  string MainContent { get; set; }
        public bool AllowDelete { get; set; }
        public  bool IsIndex { get; set; }
    }
}