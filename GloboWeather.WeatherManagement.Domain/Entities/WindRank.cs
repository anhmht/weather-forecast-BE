
using System.ComponentModel.DataAnnotations.Schema;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class WindRank : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? WindSpeed { get; set; }
        public float? Wave { get; set; }
        public int? Color { get; set; }
    }
}