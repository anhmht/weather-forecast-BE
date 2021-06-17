using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        private readonly IUnitOfWork _;
        public StatusRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }
    }
}