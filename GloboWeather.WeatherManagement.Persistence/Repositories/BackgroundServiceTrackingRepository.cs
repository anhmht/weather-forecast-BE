using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class BackgroundServiceTrackingRepository: BaseRepository<BackgroundServiceTracking>, IBackgroundServiceTrackingRepository
    {
        private readonly IUnitOfWork _;
        public BackgroundServiceTrackingRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }

        public async Task<BackgroundServiceTracking> GetLastBackgroundServiceTracking()
        {
            var result = (await _.BackgroundServiceTrackingRepository.GetAllQuery()
                .OrderByDescending(x => x.LastDownload).Take(1).ToListAsync()).FirstOrDefault();
            return result;
        }
    }
}