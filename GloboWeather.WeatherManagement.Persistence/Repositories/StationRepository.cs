using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class StationRepository : BaseRepository<Station>, IStationRepository
    {
        private readonly IUnitOfWork _;
        public StationRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }
    }
}