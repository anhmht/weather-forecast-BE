using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
        Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate);
        Task<GetEventsListResponse> GetByPageAsync(EventsListQuery query,  CancellationToken token);
        Task<EventListWithContentResponse> GetEventListByAsync(GetEventsListWithContentQuery query, CancellationToken token);
        Task<EventMostViewResponse> GetMostViewAsync(EventMostViewQuery query, CancellationToken token);
    }
}