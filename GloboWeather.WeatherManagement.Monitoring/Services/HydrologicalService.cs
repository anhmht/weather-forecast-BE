using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class HydrologicalService : IHydrologicalService
    {
        private readonly IHydrologicalRepository _hydrologicalRepository;
        private readonly ILogger<HydrologicalService> _logger;

        public HydrologicalService(IHydrologicalRepository hydrologicalRepository
        , ILogger<HydrologicalService> logger)
        {
            _hydrologicalRepository = hydrologicalRepository;
            _logger = logger;
        }
        public async Task<GetHydrologicalListResponse> GetByPagedAsync(GetHydrologicalListQuery query)
        {
            try
            {

                return await _hydrologicalRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"HydrologicalService.GetByPagedAsync error. Request data: {JsonConvert.SerializeObject(query)}");
                throw;
            }

        }
    }
}