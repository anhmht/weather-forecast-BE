using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Forum: AuditableEntity
    {
        public Guid ForumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool EnableUpDownVotes  { get; set; }
        public bool IsSupportForum { get; set; }
        public virtual ICollection<ForumCategory> ForumCategories { get; set; }
        
    }
}