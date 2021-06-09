using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class BackgroundServiceTracking
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime LastDownload { get; set; }
    }
}