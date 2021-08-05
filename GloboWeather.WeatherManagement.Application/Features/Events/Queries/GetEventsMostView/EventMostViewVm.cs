using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView
{
    public class EventMostViewVm
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public string StatusName { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedFullName { get; set; }
        public int ViewCount { get; set; }
    }
}