using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class Post : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string ImageUrls { get; set; }
        public string VideoUrls { get; set; }
        public int StatusId { get; set; } //CommonLookups namespace POST_STATUS
        public DateTime? PublicDate { get; set; } //The day that status change into "Public"
        public string ApprovedByUserName { get; set; } //User approve the post
        public string VideoUrlsIos { get; set; }

        //CreateBy: user makes action
    }
}
