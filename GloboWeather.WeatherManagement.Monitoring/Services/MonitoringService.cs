using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IMapper _mapper;
        private readonly ITramKttvRepository _tramKttvRepository;
        private readonly ILogger<MonitoringService> _logger;
        public MonitoringService(IMapper mapper,
            ITramKttvRepository tramKttvRepository
            , ILogger<MonitoringService> logger
            )
        {
            _mapper = mapper;
            _tramKttvRepository = tramKttvRepository;
            _logger = logger;
        }

        public async Task<List<TramKttvResponse>> GetTramKttvList()
        {
            try
            {
                var tmp = await _tramKttvRepository.ListAllAsync();
                foreach (var tramKttv in tmp)
                {
                    tramKttv.StationType = tramKttv.StationType.ToLower();
                }
                return _mapper.Map<List<TramKttvResponse>>(tmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MonitoringService.GetTramKttvList error");
                throw;
            }
        }

    }
}