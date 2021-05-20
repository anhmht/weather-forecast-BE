using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumCategory : AuditableEntity
    {
        public Guid ForumCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public int ForumId { get; set; }
        public  virtual  ICollection<ForumTopic> ForumTopics { get; set; }
    }
}