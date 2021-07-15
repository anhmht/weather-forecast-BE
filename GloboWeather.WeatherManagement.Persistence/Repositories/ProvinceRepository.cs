using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProvinceRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }
    }
}