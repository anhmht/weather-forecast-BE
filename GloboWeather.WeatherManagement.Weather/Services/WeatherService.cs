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

        /// <summary>
        /// get list of Weather by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<WeatherPredictionResponse> GetWeatherMinMaxByDiemId(string diemDuBaoId)
        {
            var WeatherEntity = await _weatherRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new WeatherPredictionResponse();
            if (WeatherEntity == null)
                return new WeatherPredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

            var currentDate = WeatherEntity.RefDate;
            var listWeatherTheoNgay = new List<WeatherDayResponse>();
            var WeatherTheoNgay = new WeatherDayResponse()
            {
                Date = currentDate,
                WeatherByHours = new List<WeatherHour>()
            };

            var listWeatherTheoGioTmp = new List<WeatherHour>();
            int currentDay = 0;
            var WeatherTheoThoiGianMin = new List<WeatherTime>();
            var WeatherTheoThoiGianMax = new List<WeatherTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var Weather = WeatherEntity.GetType().GetProperty($"_{i}").GetValue(WeatherEntity, null);
                var WeatherTheoGio = new WeatherHour()
                {
                    Hour = nextHour.Hour,
                    Weather = (string)Weather
                };
                listWeatherTheoGioTmp.Add(WeatherTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    WeatherTheoNgay.WeatherByHours.AddRange(listWeatherTheoGioTmp);
                    // calculate Weather min or max
                    var WeatherMinTmp = listWeatherTheoGioTmp.Min(x => x.Weather);
                    var WeatherMaxTmp = listWeatherTheoGioTmp.Max(x => x.Weather);
                  
                    listWeatherTheoNgay.Add(WeatherTheoNgay);

                    // reinnit data
                    currentDay++;
                    WeatherTheoNgay = new WeatherDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        WeatherByHours = new List<WeatherHour>()                       
                    };
                    listWeatherTheoGioTmp = new List<WeatherHour>();
                }

            }
            duBaohietDoResponse.WeatherByDays = listWeatherTheoNgay;          
            return duBaohietDoResponse;
        }
    }
}