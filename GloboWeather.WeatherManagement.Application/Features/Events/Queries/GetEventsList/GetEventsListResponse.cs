using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Helpers.Common;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public record GetEventsListResponse : ILinkedResource
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<EventListVm> Events { get; init; }
        public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
    }
}