using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostsForApproval
{
    public class GetPostsForApprovalQuery : BasePagingRequest, IRequest<GetPostsForApprovalResponse>
    {
        public List<int> StatusIds { get; set; }

        public GetPostsForApprovalQuery()
        {
            StatusIds = new List<int>();
        }
    }
}
