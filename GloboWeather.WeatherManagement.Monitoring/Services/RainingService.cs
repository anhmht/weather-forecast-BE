using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class RainingService : IRainingService
    {
        private readonly IRainRepository _rainRepository;
        private readonly ILogger<RainingService> _logger;

        public RainingService(IRainRepository rainRepository, ILogger<RainingService> logger)
        {
            _rainRepository = rainRepository;
            _logger = logger;
        }
        public async Task<GetRainListResponse> GetByPagedAsync(GetRainsListQuery query)
        {
            try
            {
                return await _rainRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"RainingService.GetByPagedAsync error. Request data: {JsonConvert.SerializeObject(query)}");
                throw;
            }
            
        }
    }
}