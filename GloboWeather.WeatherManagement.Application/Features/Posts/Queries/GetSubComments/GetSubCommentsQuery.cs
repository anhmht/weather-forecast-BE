using System;
using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetSubComments
{
    public class GetSubCommentsQuery : BasePagingRequest, IRequest<GetSubCommentsResponse>
    {
        public Guid CommentId { get; set; }
    }
}
