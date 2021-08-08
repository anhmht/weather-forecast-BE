using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class HistoryTracking : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string ObjectName { get; set; } // Table/class name
        public string Description { get; set; } //Some text describe the action
        public string OriginalData { get; set; } //Data before changing
        public string UpdatedData { get; set; } //Data after changing
        public string IpAddress { get; set; }
    }
}
