using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class MeteorologicalStation : AuditableEntity//TramKttv
    {
        [Key]
        public string StationId { get; set; }
        public string Name { get; set; }
        public float? GoogleX { get; set; }
        public float? GoogleY { get; set; }
        public int MeteorologicalStationTypeId { get; set; }
        public string GoverningBody { get; set; } //CQQuanLy
        [Required]
        public int ProvinceId { get; set; }
        public string Address { get; set; }
        public int? Hong { get; set; } //Unknown column
        public int? Regime { get; set; } //Chedo

    }
}