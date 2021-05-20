using System;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class EventRepository: BaseRepository<Event>, IEventRepository
    {
        public EventRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            var matches = _dbContext.Events.Any(e => e.Title.Equals(name) && e.DatePosted.Date.Equals(eventDate.Date));

            return Task.FromResult(matches);
        }
    }
}