using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;
using GloboWeather.WeatherManagement.Monitoring.IRepository;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class MeteorologicalService : IMeteorologicalService
    {
        private readonly IMeteorologicalRepository _meteorologicalRepository;

        public MeteorologicalService(IMeteorologicalRepository meteorologicalRepository)
        {
            _meteorologicalRepository = meteorologicalRepository;
        }
        public async Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query)
        {
            return await _meteorologicalRepository.GetByPagedAsync(query);
        }
    }
}