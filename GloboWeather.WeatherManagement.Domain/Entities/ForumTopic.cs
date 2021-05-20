using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumTopic : AuditableEntity
    {
        public  Guid ForumTopicId { get; set; }
        
        [Required]
        [MaxLength(125)]
        public string Title { get; set; }
       
        [Required]
        public string TopicText { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
       
        [MaxLength(256)]
        public string UserIP { get; set; }
        
        [Required]
        public DateTime PostedDate { get; set; }
        
        [Required]
        public int Flags { get; set; }
        
        [MaxLength(100)]
        public string EditReason { get; set; }
        
        [Required]
        public bool IsModeratorChanged { get; set; }
        
        [MaxLength(100)]
        public string DeleteReason { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; }
        
        [Required]
        public bool IsApproved { get; set; }
        
        [Required]
        public Guid ForumCategoryId { get; set; }
        
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
        
        
    }
}