using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class ForumPost : AuditableEntity
    {
        public Guid ForumPostId { get; set; }
        
        [Required]
        public DateTime PostedDate { get; set; }
        
        [Required]
        public string PostText { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [MaxLength(50)]
        public string UserIP { get; set; }
        
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
        public Guid ForumTopicId { get; set; }
        public Guid? ParentPostId { get; set; }
        public  bool IsAnwser { get; set; }
    }
}