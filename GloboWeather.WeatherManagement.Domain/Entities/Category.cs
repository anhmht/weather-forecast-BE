using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Category: AuditableEntity
    {
        public  Guid CategoryId { get; set; }
        public  string Name { get; set; }
    }
}