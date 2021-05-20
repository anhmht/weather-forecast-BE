using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Event : AuditableEntity
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public  string Content { get; set; }
        public  string ImageUrl { get; set; }
        public  DateTime DatePosted { get; set;}
        public  int Status { get; set; }
        public  Guid CategoryId { get; set; }
        public  Category Category { get; set; }
    }
}