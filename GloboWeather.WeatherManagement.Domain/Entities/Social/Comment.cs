using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class Comment : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string ImageUrls { get; set; }
        public string VideoUrls { get; set; }
        public int StatusId { get; set; } //CommonLookups namespace POST_STATUS
        public DateTime? PublicDate { get; set; } //The day that status change into "Public"
        public string ApprovedByUserName { get; set; } //User approve the comment
        public Guid? AnonymousUserId { get; set; } //If social user -> this field will be empty
        public Guid? ParentCommentId { get; set; }
        public string VideoUrlsIos { get; set; }

        //CreateBy: user makes action
    }
}
