using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy
{
    public class GetEventListByResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<EventListCateStatusVm> Events { get; set; }
    }
}
