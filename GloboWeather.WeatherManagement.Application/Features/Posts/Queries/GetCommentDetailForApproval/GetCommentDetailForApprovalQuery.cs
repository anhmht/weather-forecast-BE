using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval
{
    public class GetCommentDetailForApprovalQuery : IRequest<GetCommentDetailForApprovalResponse>
    {
        public Guid Id { get; set; }
    }
}
