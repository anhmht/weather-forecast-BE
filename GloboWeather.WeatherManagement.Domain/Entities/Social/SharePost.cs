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
        public Guid UserId { get; set; } //User share the post
        public string ShareTo { get; set; }
    }
}
