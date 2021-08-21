using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval
{
    public class GetPostDetailForApprovalQuery : IRequest<GetPostDetailForApprovalResponse>
    {
        public Guid Id { get; set; }
    }
}
