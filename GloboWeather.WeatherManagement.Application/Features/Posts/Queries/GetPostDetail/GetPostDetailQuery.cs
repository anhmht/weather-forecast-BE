using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail
{
    public class GetPostDetailQuery : IRequest<GetPostDetailResponse>
    {
        public Guid Id { get; set; }
    }
}
