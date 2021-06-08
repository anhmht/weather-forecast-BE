using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IMapper _mapper;
        private readonly ITramKttvRepository _tramKttvRepository;
        private readonly IRainRepository _rainRepository;

        public MonitoringService(IMapper mapper,
            ITramKttvRepository tramKttvRepository,
            IRainRepository rainRepository)
        {
            _mapper = mapper;
            _tramKttvRepository = tramKttvRepository;
            _rainRepository = rainRepository;
        }

        public async Task<List<TramKttvResponse>> GetTramKttvList()
        {
            try
            {
                var tmp = await _tramKttvRepository.ListAllAsync();
                return _mapper.Map<List<TramKttvResponse>>(tmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetRainMinMaxResponse>> GetRainMinMax()
        {
            try
            {
                return await _rainRepository.GetMinMaxRain();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}