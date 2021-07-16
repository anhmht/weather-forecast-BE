using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class EventDetailVm
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid StatusId { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public StatusDto Status { get; set; }
        public List<EventDocument> Documents { get; set; }
    }
}