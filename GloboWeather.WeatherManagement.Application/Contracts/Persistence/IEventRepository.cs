using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
        Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate);
        Task<GetEventsListResponse> GetByPageAsync(GetEventsListQuery query,  CancellationToken token);
        Task<List<Event>> GetEventListByAsync(Guid categoryId, Guid statusId, CancellationToken token);
        Task<EventMostViewResponse> GetMostViewAsync(EventMostViewQuery query, CancellationToken token);
    }
}