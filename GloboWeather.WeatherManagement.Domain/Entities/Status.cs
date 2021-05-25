using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Status : AuditableEntity
    {
        public Guid StatusId { get; set; }
        public string Name { get; set; }
        public  ICollection<Event> Events { get; set; }
    }
}