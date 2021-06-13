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
    public class HumidityService : IHumidityService
    {
        private readonly IMapper _mapper;
        private readonly IHumidityRepository _humidityRepository;       

        public HumidityService(IMapper mapper,
            IHumidityRepository humidityRepository
            )
        {
            _mapper = mapper;
            _humidityRepository = humidityRepository;          
        }     
        /// <summary>
        /// get list of Humidity by DiemId
        /// </summary>
        /// <param name="HumidityId"></param>
        /// <returns></returns>
        public async Task<HumidityPredictionResponse> GetHumidityMinMaxByDiemId(string diemId)
        {
            var HumidityEntity = await _humidityRepository.GetByIdAsync(diemId);

            var duBaohietDoResponse = new HumidityPredictionResponse();
            if (HumidityEntity == null)
                return new HumidityPredictionResponse();

            duBaohietDoResponse.DiemId = diemId;

            var currentDate = HumidityEntity.RefDate;
            var listHumidityTheoNgay = new List<HumidityDayResponse>();
            var HumidityTheoNgay = new HumidityDayResponse()
            {
                Date = currentDate,
                HumidityByHours = new List<HumidityHour>()
            };

            var listHumidityTheoGioTmp = new List<HumidityHour>();
            int currentDay = 0;
            var HumidityTheoThoiGianMin = new List<HumidityTime>();
            var HumidityTheoThoiGianMax = new List<HumidityTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var Humidity = HumidityEntity.GetType().GetProperty($"_{i}").GetValue(HumidityEntity, null);
                var HumidityTheoGio = new HumidityHour()
                {
                    Hour = nextHour.Hour,
                    Humidity = (int)Humidity
                };
                listHumidityTheoGioTmp.Add(HumidityTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    HumidityTheoNgay.HumidityByHours.AddRange(listHumidityTheoGioTmp);
                    // calculate Humidity min or max
                    var HumidityMinTmp = listHumidityTheoGioTmp.Min(x => x.Humidity);
                    var HumidityMaxTmp = listHumidityTheoGioTmp.Max(x => x.Humidity);
                    HumidityTheoNgay.HumidityMins.AddRange(listHumidityTheoGioTmp.Where(x => x.Humidity == HumidityMinTmp));
                    HumidityTheoNgay.HumidityMaxs.AddRange(listHumidityTheoGioTmp.Where(x => x.Humidity == HumidityMaxTmp));
                    HumidityTheoNgay.HumidityMin = HumidityMinTmp;
                    HumidityTheoNgay.HumidityMax = HumidityMaxTmp;

                    listHumidityTheoNgay.Add(HumidityTheoNgay);

                    // reinnit data
                    currentDay++;
                    HumidityTheoNgay = new HumidityDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        HumidityByHours = new List<HumidityHour>(),
                        HumidityMaxs = new List<HumidityHour>(),
                        HumidityMins = new List<HumidityHour>()
                    };
                    listHumidityTheoGioTmp = new List<HumidityHour>();
                }

            }
            duBaohietDoResponse.HumidityByDays = listHumidityTheoNgay;
            duBaohietDoResponse.HumidityMin = listHumidityTheoNgay.Min(x => x.HumidityMins.Min(x => x.Humidity));
            duBaohietDoResponse.HumidityMax = listHumidityTheoNgay.Max(x => x.HumidityMaxs.Max(x => x.Humidity));
            return duBaohietDoResponse;
        }

        public async Task<HumidityResponse> GetHumidityBy(string diemDuBaoId)
        {
            var humidityEntity = await _humidityRepository.GetByIdAsync(diemDuBaoId);
            return _mapper.Map<HumidityResponse>(humidityEntity);
        }

        public async Task<List<HumidityResponse>> ListAllAsync()
        {
            var data = await _humidityRepository.ListAllAsync();
            var result = new List<HumidityResponse>();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<HumidityResponse>(item));
            }
            return result;
        }

    }
}