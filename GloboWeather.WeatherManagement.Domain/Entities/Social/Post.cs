using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class Post : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; } //User post
        public string Content { get; set; }
        public string ImageUrls { get; set; }
        public string VideoUrls { get; set; }
        public int StatusId { get; set; } //CommonLookups namespace POST_STATUS
        public DateTime? PublicDate { get; set; } //The day that status change into "Public"
        public Guid? ApprovedByUserId { get; set; } //User approve the post
    }
}
