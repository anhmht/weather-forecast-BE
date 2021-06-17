using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class SitePageRepository : BaseRepository<SitePage>, ISitePageRepository
    {
        private readonly IUnitOfWork _;
        public SitePageRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }
    }
}