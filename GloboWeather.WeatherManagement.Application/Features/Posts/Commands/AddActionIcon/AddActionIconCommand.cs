using System;
using System.Text.Json.Serialization;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon
{
    public class AddActionIconCommand : IRequest
    {
        public Guid Id { get; set; }
        public int IconId { get; set; }
        [JsonIgnore]
        public bool IsPost { get; set; } //IsPost = true: use for making action on the Post, else use for making action on the Comment
    }
}
