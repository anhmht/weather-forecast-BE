using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ForumPostRepository : BaseRepository<ForumPost>, IForumPostRepository
    {
        private readonly IUnitOfWork _;

        public ForumPostRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }
    }
}