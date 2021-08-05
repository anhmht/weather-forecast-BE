using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView
{
    public class EventMostViewResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<EventMostViewVm> Events { get; set; }
    }
}