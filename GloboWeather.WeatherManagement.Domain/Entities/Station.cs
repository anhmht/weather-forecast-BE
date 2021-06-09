using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Station : AuditableEntity
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        public float GoogleX { get; set; }
        public float GoogleY { get; set; }
        [DefaultValue(false)]
        public bool IsSystem { get; set; }
    }
}