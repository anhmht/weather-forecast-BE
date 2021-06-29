using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class District : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
    }
}