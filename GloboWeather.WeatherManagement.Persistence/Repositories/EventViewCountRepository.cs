using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class EventViewCountRepository: BaseRepository<EventViewCount>, IEventViewCountRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventViewCountRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddEventViewCountAsync(Guid eventId)
        {
            var eventViewCount = await _unitOfWork.EventViewCountRepository.GetByIdAsync(eventId);
            if (eventViewCount == null)
            {
                _unitOfWork.EventViewCountRepository.Add(new EventViewCount()
                {
                    EventId = eventId,
                    LastTimeView = DateTime.Now,
                    ViewCount = 1
                });
            }
            else
            {
                eventViewCount.ViewCount++;
                eventViewCount.LastTimeView = DateTime.Now;
                _unitOfWork.EventViewCountRepository.Update(eventViewCount);
            }

            await _unitOfWork.CommitAsync();
        }
    }
}