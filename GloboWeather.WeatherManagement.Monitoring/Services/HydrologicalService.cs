using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Monitoring.IRepository;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class HydrologicalService : IHydrologicalService
    {
        private readonly IHydrologicalRepository _hydrologicalRepository;

        public HydrologicalService(IHydrologicalRepository hydrologicalRepository)
        {
            _hydrologicalRepository = hydrologicalRepository;
        }
        public async Task<GetHydrologicalListResponse> GetByPagedAsync(GetHydrologicalListQuery query)
        {
            try
            {
                return await _hydrologicalRepository.GetByPagedAsync(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}