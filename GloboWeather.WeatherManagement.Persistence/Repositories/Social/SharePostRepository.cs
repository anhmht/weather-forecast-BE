using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Persistence.Repositories.Social
{
    public class SharePostRepository : BaseRepository<SharePost>, ISharePostRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SharePostRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

    }
}