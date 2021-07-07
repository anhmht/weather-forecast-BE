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
        public MonitoringService(IMapper mapper,
            ITramKttvRepository tramKttvRepository
            )
        {
            _mapper = mapper;
            _tramKttvRepository = tramKttvRepository;
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
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}