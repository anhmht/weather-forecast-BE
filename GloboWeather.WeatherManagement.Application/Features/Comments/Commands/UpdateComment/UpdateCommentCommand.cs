using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<string> VideoUrls { get; set; }
    }
}
