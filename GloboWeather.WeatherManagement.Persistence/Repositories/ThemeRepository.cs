using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ThemeRepository : BaseRepository<Theme>, IThemeRepository
    {
        private readonly IUnitOfWork _;
        public ThemeRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }
    }
}