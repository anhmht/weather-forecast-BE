using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IEventViewCountRepository : IAsyncRepository<EventViewCount>
    {
        Task AddEventViewCountAsync(Guid eventId);
    }
}