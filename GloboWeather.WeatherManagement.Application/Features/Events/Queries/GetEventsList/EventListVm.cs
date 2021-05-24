using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class EventListVm
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public int Status { get; set; }
    }
}