using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Persistence.Repositories.Social
{
    public class PostActionIconRepository : BaseRepository<PostActionIcon>, IPostActionIconRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostActionIconRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

    }
}