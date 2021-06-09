using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class WeatherInformation : AuditableEntity
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string StationId { get; set; }
        [Required]
        public DateTime RefDate { get; set; }
        /// <summary>
        /// Do am
        /// </summary>
        public string Humidity { get; set; }
        /// <summary>
        /// Gio giat
        /// </summary>
        public string WindLevel { get; set; }
        /// <summary>
        /// Huong gio
        /// </summary>
        public string WindDirection { get; set; }
        /// <summary>
        /// Toc do gio
        /// </summary>
        public string WindSpeed { get; set; }
        /// <summary>
        /// Luong mua
        /// </summary>
        public string RainAmount { get; set; }
        /// <summary>
        /// Nhiet do
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// Thoi tiet
        /// </summary>
        public string Weather { get; set; }
    }
}