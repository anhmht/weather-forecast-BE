using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ScenarioActionDetail : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ActionId { get; set; }
        public int ScenarioActionTypeId { get; set; }
        public int? ActionTypeId { get; set; }
        public int? MethodId { get; set; }
        public string Content { get; set; }
        public int? Duration { get; set; }
        public int? Time { get; set; }
        public int? PositionId { get; set; }
        public bool? CustomPosition { get; set; }
        public int? Left { get; set; }
        public int? Top { get; set; }
        public bool? IsDisplay { get; set; }
        public int? StartTime { get; set; }
        public int? Width { get; set; }

        /// <summary>
        /// List of Icon URL, separator by ";"
        /// </summary>
        public string IconUrls { get; set; }
        public string PlaceId { get; set; }
        public bool? IsProvince { get; set; }
        public bool? IsEnableIcon { get; set; }
    }
}
