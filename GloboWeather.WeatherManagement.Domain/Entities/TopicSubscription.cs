using System;
using System.Net.Http.Headers;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class TopicSubscription : AuditableEntity
    {
        public Guid TopicSubscriptionId { get; set; }
        public  Guid UserId { get; set; }
        public  Guid ForumTopicId { get; set; }
    }
}