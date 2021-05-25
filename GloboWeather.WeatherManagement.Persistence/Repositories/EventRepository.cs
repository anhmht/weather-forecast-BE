using System;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GetEventsListResponse> GetByPageAsync(int limit, int page, CancellationToken token)
        {
            var events = await _dbContext.Events
                .Include(e => e.Category)
                .Include(e => e.Status)
                .AsNoTracking()
                .OrderBy(p => p.DatePosted)
                .PaginateAsync(page, limit, token);
            return new GetEventsListResponse
            {
                CurrentPage = events.CurrentPage,
                TotalPages = events.TotalPages,
                TotalItems = events.TotalItems,
                Events = events.Items.Select(e => new EventListVm
                {
                    EventId = e.EventId,
                    StatusName = e.Status.Name,
                    Title = e.Title,
                    CategoryName = e.Category.Name,
                    DatePosted = e.DatePosted,
                    ImageUrl = e.ImageUrl
                }).ToList()
            };
            
        }
    }
}