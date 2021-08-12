using System;

namespace GloboWeather.WeatherManagement.Application.Models.Social
{
    public class ActionIconDto
    {
        public Guid? PostId { get; set; }
        public int? PostIcon { get; set; }
        public string PostActionUserName { get; set; }
        public Guid? CommentId { get; set; }
        public int? CommentIcon { get; set; }
        public string CommentActionUserName { get; set; }
    }
}
