using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ExtremePhenomenonDetailRepository : BaseRepository<ExtremePhenomenonDetail>, IExtremePhenomenonDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExtremePhenomenonDetailRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }
    }
}