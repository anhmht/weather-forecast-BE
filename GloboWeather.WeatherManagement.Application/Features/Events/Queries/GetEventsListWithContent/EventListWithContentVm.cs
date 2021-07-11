using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public record EventListWithContentVm
    {
        public Guid EventId { get; init; }
        public string Title { get; init; }
        public string ImageUrl { get; init; }
        public DateTime DatePosted { get; init; }
        public string Content { get; init; }
    }
}