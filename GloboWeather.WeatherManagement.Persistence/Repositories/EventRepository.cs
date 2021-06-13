using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId;
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

        public async Task<GetEventsListResponse> GetByPageAsync(GetEventsListQuery query,  CancellationToken token)
        {
            var events =  _dbContext.Events as IQueryable<Event>;
            if (query.CategoryId.HasValue)
            {
                events = events.Where(e => e.CategoryId == query.CategoryId);
            }
            if (query.StatusId.HasValue)
            {
                events = events.Where(e => e.StatusId == query.StatusId);
            }
            var collections = await  events
                            .Include(e => e.Category)
                            .Include(e => e.Status)
                            .AsNoTracking()
                            .OrderByDescending(p => p.DatePosted)
                            .PaginateAsync(query.Page, query.Limit, token);
            return new GetEventsListResponse
            {
                CurrentPage = collections.CurrentPage,
                TotalPages = collections.TotalPages,
                TotalItems = collections.TotalItems,
                Events = collections.Items.Select(e => new EventListVm
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

        public async Task<List<Event>> GetEventListByAsync(Guid categoryId, Guid statusId, CancellationToken token)
        {
            return await _dbContext.Events.Where(e => e.CategoryId == categoryId
                                                         && e.StatusId == statusId).ToListAsync(token);
            
        }
    }
}