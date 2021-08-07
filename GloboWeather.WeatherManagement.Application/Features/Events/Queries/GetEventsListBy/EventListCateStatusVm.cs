using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy
{
    public record EventListCateStatusVm
    {
        public Guid EventId { get; init; }
        public string Title { get; init; }
        public string ImageUrl { get; init; }
        public DateTime DatePosted { get; init; }
    }
}