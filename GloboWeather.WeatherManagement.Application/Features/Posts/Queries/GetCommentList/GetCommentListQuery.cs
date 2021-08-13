using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList
{
    public class GetCommentListQuery : IRequest<GetCommentListResponse>
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public Guid PostId { get; set; }
    }
}
