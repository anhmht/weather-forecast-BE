using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumCategory : AuditableEntity
    {
        public Guid ForumCategoryId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }
        
        [Required]
        public int Rank { get; set; }
        
        [Required]
        public int ForumId { get; set; }
        
        public  virtual  ICollection<ForumTopic> ForumTopics { get; set; }
    }
}