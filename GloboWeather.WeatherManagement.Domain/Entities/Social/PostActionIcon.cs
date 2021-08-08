using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class PostActionIcon : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? PostId { get; set; } //If this field has value, it means user makes action on the post -> CommentId field will be empty
        public Guid? CommentId { get; set; } //If this field has value, it means user makes action on the comment -> PostId field will be empty
        public int IconId { get; set; } //Mapping with CommonLookups namespace ACTION_ICON
        
        //User makes action: CreateBy
    }
}
