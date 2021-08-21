using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public class EventListWithContentResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<EventListWithContentVm> Events { get; set; }
    }

    public class EventListWithContentVm
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public string Content { get; set; }
    }
}