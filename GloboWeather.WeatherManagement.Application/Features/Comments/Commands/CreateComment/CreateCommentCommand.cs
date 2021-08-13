using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<Guid>
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<string> VideoUrls { get; set; }
        public AnonymousUserRequest AnonymousUser { get; set; } //If user logged in -> this field will be empty
    }

    public class AnonymousUserRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
