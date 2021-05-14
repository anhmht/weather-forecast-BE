using System;

namespace GloboWeather.WeatherManagement.Domain.Common
{
    public class AuditableEntity
    {
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}