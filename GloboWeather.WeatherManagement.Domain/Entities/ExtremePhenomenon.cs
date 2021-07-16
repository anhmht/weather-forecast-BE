using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ExtremePhenomenon : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        public int ProvinceId { get; set; }
        
        public Guid DistrictId { get; set; }
        
        public DateTime Date  { get; set; }
        
    }
}