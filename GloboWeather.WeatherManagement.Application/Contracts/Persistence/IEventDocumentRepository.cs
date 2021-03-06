using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IEventDocumentRepository : IAsyncRepository<EventDocument>
    {
        Task<List<EventDocument>> GetByEventIdAsync(Guid eventId);
    }
}