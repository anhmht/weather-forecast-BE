using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Status : AuditableEntity
    {
        [Key]
        public Guid StatusId { get; set; }
        public string Name { get; set; }
  
    }
}