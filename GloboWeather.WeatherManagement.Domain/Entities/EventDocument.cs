
using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class EventDocument : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}