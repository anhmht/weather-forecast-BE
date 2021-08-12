using System;
using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class DeleteFile
    {
        [Key]
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public Guid DeleteId { get; set; }
        public string ContainerName { get; set; }
        public string FileUrl { get; set; }
    }
}
