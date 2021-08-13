using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList
{
    public class GetCommentListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<CommentVm> Comments { get; set; }
    }
}
