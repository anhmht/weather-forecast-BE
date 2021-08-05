using System;
using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class EventViewCount
    {
        [Key]
        public Guid EventId { get; set; }
        public int ViewCount { get; set; }
        public DateTime LastTimeView { get; set; }
    }
}
