using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Helpers.Common;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListResponse : ILinkedResource
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<EventListVm> Events { get; set; }
        public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
    }
}