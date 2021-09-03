using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class MeteorologicalService : IMeteorologicalService
    {
        private readonly IMeteorologicalRepository _meteorologicalRepository;
        private readonly ILogger<MeteorologicalService> _logger;

        public MeteorologicalService(IMeteorologicalRepository meteorologicalRepository
        , ILogger<MeteorologicalService> logger)
        {
            _meteorologicalRepository = meteorologicalRepository;
            _logger = logger;
        }
        public async Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query)
        {
            try
            {
                return await _meteorologicalRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"MeteorologicalService.GetByPagedAsync error. Request data: {JsonConvert.SerializeObject(query)}");
                throw;
            }
        }
    }
}