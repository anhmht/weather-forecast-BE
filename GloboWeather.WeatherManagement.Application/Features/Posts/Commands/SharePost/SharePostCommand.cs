using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.SharePost
{
    public class SharePostCommand : IRequest<Guid>
    {
        public Guid PostId { get; set; }
        public string ShareTo { get; set; }
    }
}
