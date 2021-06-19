using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
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

        public async Task<GetEventsListResponse> GetByPageAsync(GetEventsListQuery query,  CancellationToken token)
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
                    ImageUrl = e.ImageUrl
                }).ToList()
            };
            
        }

        public async Task<List<Event>> GetEventListByAsync(Guid categoryId, Guid statusId, CancellationToken token)
        {
            return (await _unitOfWork.EventRepository.GetWhereAsync(e => e.CategoryId == categoryId
                                                         && e.StatusId == statusId, token)).ToList();

        }

    }
}