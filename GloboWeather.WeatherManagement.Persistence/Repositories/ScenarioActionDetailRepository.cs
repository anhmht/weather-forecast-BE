using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ScenarioActionDetailRepository : BaseRepository<ScenarioActionDetail>, IScenarioActionDetailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScenarioActionDetailRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

    }
}