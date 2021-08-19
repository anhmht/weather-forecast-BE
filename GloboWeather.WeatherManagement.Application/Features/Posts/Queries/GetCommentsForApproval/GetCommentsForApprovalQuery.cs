using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval
{
    public class GetCommentsForApprovalQuery : BasePagingRequest, IRequest<GetCommentsForApprovalResponse>
    {
        public List<int> StatusIds { get; set; }

        public GetCommentsForApprovalQuery()
        {
            StatusIds = new List<int>();
        }
    }
}
