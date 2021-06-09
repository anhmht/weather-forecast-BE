using GloboWeather.WeatherManagement.Domain.Common;
using System;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class WeatherMinMaxData : AuditableEntity
    {
        public Guid Id { get; set; }

        public string DiemId { get; set; }
        public int Type { get; set; }

        public DateTime RefDate { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}
