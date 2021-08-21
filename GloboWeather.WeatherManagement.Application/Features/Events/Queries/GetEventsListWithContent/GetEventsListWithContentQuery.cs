using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public class GetEventsListWithContentQuery : EventsListQuery, IRequest<EventListWithContentResponse>
    {
        
    }
}