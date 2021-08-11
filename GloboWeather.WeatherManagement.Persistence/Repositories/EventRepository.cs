using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class EventRepository: BaseRepository<Event>, IEventRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            return await _unitOfWork.EventRepository.GetWhereQuery(e => e.Title.Equals(name) && e.DatePosted.Date.Equals(eventDate.Date)).AnyAsync();
        }

        public async Task<GetEventsListResponse> GetByPageAsync(EventsListQuery query, CancellationToken token)
        {
            var events =  _unitOfWork.EventRepository.GetAllQuery();
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
                    ImageUrl = e.ImageUrl,
                    CreatedBy = e.CreateBy
                }).ToList()
            };
            
        }

        public async Task<List<Event>> GetEventListByAsync(Guid categoryId, Guid statusId, CancellationToken token)
        {
            return (await _unitOfWork.EventRepository.GetWhereAsync(e => e.CategoryId == categoryId
                                                         && e.StatusId == statusId, token)).ToList();

        }

        public async Task<EventMostViewResponse> GetMostViewAsync(EventMostViewQuery query, CancellationToken token)
        {
            var dayLimit = query.DayNumber == 0 ? DateTime.MinValue : DateTime.Now.Date.AddDays(-query.DayNumber);
            var eventsQuery = (from e in _unitOfWork.EventRepository.GetAllQuery()
                    .Include(e => e.Category)
                    .Include(e => e.Status)
                    .AsNoTracking()
                join c in _unitOfWork.EventViewCountRepository.GetAllQuery().AsNoTracking()
                    on e.EventId equals c.EventId
                where e.DatePosted >= dayLimit && e.StatusId == EventStatus.Publish
                select new EventMostViewVm()
                {
                    EventId = e.EventId,
                    ImageUrl = e.ImageUrl,
                    CreatedBy = e.CreateBy,
                    CategoryName = e.Category.Name,
                    DatePosted = e.DatePosted,
                    StatusName = e.Status.Name,
                    Title = e.Title,
                    ViewCount = c.ViewCount
                }).OrderByDescending(x => x.ViewCount);

            var collections = await eventsQuery.PaginateAsync(query.Page, query.Limit, token);
            return new EventMostViewResponse
            {
                CurrentPage = collections.CurrentPage,
                TotalPages = collections.TotalPages,
                TotalItems = collections.TotalItems,
                Events = collections.Items.ToList()
            };
        }

    }
}