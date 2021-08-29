using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class SocialNotification : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public string Receiver { get; set; } //UserName
        public bool IsRead { get; set; }
        public string Action { get; set; } //Insert, update, delete, like, share...
        public string Description { get; set; }
        public Guid? AnonymousUserId { get; set; }
    }
}
