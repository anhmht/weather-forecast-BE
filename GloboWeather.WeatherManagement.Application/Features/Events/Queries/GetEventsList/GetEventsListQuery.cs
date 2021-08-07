using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQuery : EventsListQuery, IRequest<GetEventsListResponse>
    {
        
    }
    public class EventsListQuery
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? StatusId { get; set; }
    }
}