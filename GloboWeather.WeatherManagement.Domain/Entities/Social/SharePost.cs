using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class SharePost : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string ShareTo { get; set; }
    }
}
