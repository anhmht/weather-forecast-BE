using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public record EventListVm
    {
        public Guid EventId { get; init; }
        public string Title { get; init; }
        public string ImageUrl { get; init; }
        public DateTime DatePosted { get; init; }
        public string StatusName { get; init; }
        public string CategoryName { get; init; }
    }
}