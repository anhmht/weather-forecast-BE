using System;
using System.Collections.Generic;
using System.Globalization;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumTopic : AuditableEntity
    {
        public  Guid ForumTopicId { get; set; }

        public string Title { get; set; }
        public string TopicText { get; set; }
        public Guid UserId { get; set; }
        public string UserIP { get; set; }
        public DateTime PostedDate { get; set; }
        public int Flags { get; set; }
        public string EditReason { get; set; }
        public bool IsModeratorChanged { get; set; }
        public string DeleteReason { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
        public Guid ForumCategoryId { get; set; }
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
        
        
    }
}