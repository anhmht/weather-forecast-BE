using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
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
        public async Task<List<GetHydrologicalResponse>> GetHydrologicalAsync()
        {
            try
            {
                return await _hydrologicalRepository.GetHydrologicalAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}