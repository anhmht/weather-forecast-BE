using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class EventDocumentRepository : BaseRepository<EventDocument>, IEventDocumentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventDocumentRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventDocument>> GetByEventIdAsync(Guid eventId)
        {
            return (await _unitOfWork.EventDocumentRepository.GetWhereAsync(x => x.EventId == eventId)).ToList();
        }
    }
}