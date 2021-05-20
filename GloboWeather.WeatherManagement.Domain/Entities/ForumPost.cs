using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumPost : AuditableEntity
    {
        public Guid ForumPostId { get; set; }
        public DateTime PostedDate { get; set; }
        public string PostText { get; set; }
        public Guid UserId { get; set; }
        public string UserIP { get; set; }
        public int Flags { get; set; }
        public string EditReason { get; set; }
        public bool IsModeratorChanged { get; set; }
        public string DeleteReason { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
        public Guid ForumTopicId { get; set; }
        public Guid? ParentPostId { get; set; }
        public  bool IsAnwser { get; set; }
    }
}