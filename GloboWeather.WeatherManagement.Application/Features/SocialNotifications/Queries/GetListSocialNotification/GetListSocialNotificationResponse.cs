using System;
using GloboWeather.WeatherManagement.Application.Responses;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification
{
    public class GetListSocialNotificationResponse : BasePagingResponse<SocialNotificationVm>
    {
    }

    public class SocialNotificationVm
    {
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string Receiver { get; set; } //UserName
        public bool IsRead { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public string FromUserShortName { get; set; }
        public string FromUserFullName { get; set; }
        public string FromUserAvatar { get; set; }
        [JsonIgnore]
        public Guid? AnonymousUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
    }
}
