using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class MeteorologicalStationType : AuditableEntity//TramKttv
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}