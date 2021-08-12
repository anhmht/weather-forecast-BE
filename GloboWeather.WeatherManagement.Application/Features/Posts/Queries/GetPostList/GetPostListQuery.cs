using System.Text.Json.Serialization;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList
{
    public class GetPostListQuery : IRequest<GetPostListResponse>
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public int CommentLimit { get; set; } //Limit number of comment in each post
        [JsonIgnore]
        public bool IsUserTimeLine { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
    }
}
