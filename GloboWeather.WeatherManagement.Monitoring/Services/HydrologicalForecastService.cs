using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class HydrologicalForecastService : IHydrologicalForecastService
    {
        private readonly IHydrologicalForecastRepository _hydrologicalForecastRepository;
        private readonly ILogger<HydrologicalForecastService> _logger;

        public HydrologicalForecastService(IHydrologicalForecastRepository hydrologicalForecastRepository
        , ILogger<HydrologicalForecastService> logger)
        {
            _hydrologicalForecastRepository = hydrologicalForecastRepository;
            _logger = logger;
        }
        public async Task<GetHydrologicalForecastListResponse> GetByPagedAsync(GetHydrologicalForecastListQuery query)
        {
            try
            {
                return await _hydrologicalForecastRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"HydrologicalForecastService.GetByPagedAsync error. Request data: {JsonConvert.SerializeObject(query)}");
                throw;
            }

        }
    }
}