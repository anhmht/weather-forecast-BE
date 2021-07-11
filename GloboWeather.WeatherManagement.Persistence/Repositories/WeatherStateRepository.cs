using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class WeatherStateRepository : BaseRepository<WeatherState>, IWeatherStateRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public WeatherStateRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }
    }
}