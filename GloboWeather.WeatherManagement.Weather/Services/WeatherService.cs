using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IMapper _mapper;
        private readonly IDiemDuBaoRepository _diemDuBaoRepository;
        private readonly ITemperatureRepository _temperatureRepository;
        private readonly IWeatherRepository _weatherRepository;
        public WeatherService(IMapper mapper,
            IDiemDuBaoRepository diemDuBaoRepository
            , ITemperatureRepository temperatureRepository, 
            IWeatherRepository weatherRepository)
        {
            _mapper = mapper;
            _diemDuBaoRepository = diemDuBaoRepository;
            _temperatureRepository = temperatureRepository;
            _weatherRepository = weatherRepository;
        }
        public async Task<List<DiemDuBaoResponse>> GetDiemDuBaosList()
        {
            var diemdubaoEntityList = await _diemDuBaoRepository.ListAllAsync();

            return _mapper.Map<List<DiemDuBaoResponse>>(diemdubaoEntityList);

        }

       

        public async Task<WeatherResponse> GetWeatherBy(string diemDuBaoId)
        {
            var weatherEntity = await _weatherRepository.GetByIdAsync(diemDuBaoId);
            return _mapper.Map<WeatherResponse>(weatherEntity);
        }

    }
}