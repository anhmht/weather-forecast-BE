using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;
using GloboWeather.WeatherManagement.Monitoring.IRepository;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class HydrologicalForecastService : IHydrologicalForecastService
    {
        private readonly IHydrologicalForecastRepository _hydrologicalForecastRepository;

        public HydrologicalForecastService(IHydrologicalForecastRepository hydrologicalForecastRepository)
        {
            _hydrologicalForecastRepository = hydrologicalForecastRepository;
        }
        public async Task<GetHydrologicalForecastListResponse> GetByPagedAsync(GetHydrologicalForecastListQuery query)
        {
            try
            {
                return await _hydrologicalForecastRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}