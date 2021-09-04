using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class RainingService : IRainingService
    {
        private readonly IRainRepository _rainRepository;

        public RainingService(IRainRepository rainRepository)
        {
            _rainRepository = rainRepository;
        }
        public async Task<GetRainListResponse> GetByPagedAsync(GetRainsListQuery query)
        {
            return await _rainRepository.GetByPagedAsync(query);
        }
    }
}