using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval
{
    public class GetPostDetailForApprovalResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
        public string CreateBy { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorShortName { get; set; }
        public string CreatorAvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public List<string> ListImageUrl { get; set; }
        public List<string> ListVideoUrl { get; set; }
        public List<string> ListVideoUrlIos { get; set; }
    }

}
