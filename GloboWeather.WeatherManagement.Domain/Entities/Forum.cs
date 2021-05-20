using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Forum: AuditableEntity
    {
        public Guid ForumId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }
        
        public bool EnableUpDownVotes  { get; set; }
        
        public bool IsSupportForum { get; set; }
        
        public virtual ICollection<ForumCategory> ForumCategories { get; set; }
        
    }
}