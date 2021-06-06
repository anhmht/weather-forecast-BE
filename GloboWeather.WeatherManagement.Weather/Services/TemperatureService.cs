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
    public class TemperatureService : ITemperatureService
    {
        private readonly IMapper _mapper;
        private readonly ITemperatureRepository _temperatureRepository;       

        public TemperatureService(IMapper mapper,
            ITemperatureRepository temperatureRepository
            )
        {
            _mapper = mapper;
            _temperatureRepository = temperatureRepository;          
        }     
        /// <summary>
        /// get list of Temperature by DiemId
        /// </summary>
        /// <param name="TemperatureId"></param>
        /// <returns></returns>
        public async Task<TemperaturePredictionResponse> GetTemperatureMinMaxByDiemId(string diemId)
        {
            var TemperatureEntity = await _temperatureRepository.GetByIdAsync(diemId);

            var duBaohietDoResponse = new TemperaturePredictionResponse();
            if (TemperatureEntity == null)
                return new TemperaturePredictionResponse();

            duBaohietDoResponse.DiemId = diemId;

            var currentDate = TemperatureEntity.RefDate;
            var listTemperatureTheoNgay = new List<TemperatureDayResponse>();
            var TemperatureTheoNgay = new TemperatureDayResponse()
            {
                Date = currentDate,
                TemperatureByHours = new List<TemperatureByHour>()
            };

            var listTemperatureTheoGioTmp = new List<TemperatureByHour>();
            int currentDay = 0;
            var TemperatureTheoThoiGianMin = new List<TemperatureTime>();
            var TemperatureTheoThoiGianMax = new List<TemperatureTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var Temperature = TemperatureEntity.GetType().GetProperty($"_{i}").GetValue(TemperatureEntity, null);
                var TemperatureTheoGio = new TemperatureByHour()
                {
                    Hour = nextHour.Hour,
                    Temperature = (int)Temperature
                };
                listTemperatureTheoGioTmp.Add(TemperatureTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    TemperatureTheoNgay.TemperatureByHours.AddRange(listTemperatureTheoGioTmp);
                    // calculate Temperature min or max
                    var TemperatureMinTmp = listTemperatureTheoGioTmp.Min(x => x.Temperature);
                    var TemperatureMaxTmp = listTemperatureTheoGioTmp.Max(x => x.Temperature);
                    TemperatureTheoNgay.TemperatureMins.AddRange(listTemperatureTheoGioTmp.Where(x => x.Temperature == TemperatureMinTmp));
                    TemperatureTheoNgay.TemperatureMaxs.AddRange(listTemperatureTheoGioTmp.Where(x => x.Temperature == TemperatureMaxTmp));
                    TemperatureTheoNgay.TemperatureMin = TemperatureMinTmp;
                    TemperatureTheoNgay.TemperatureMax = TemperatureMaxTmp;

                    listTemperatureTheoNgay.Add(TemperatureTheoNgay);

                    // reinnit data
                    currentDay++;
                    TemperatureTheoNgay = new TemperatureDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        TemperatureByHours = new List<TemperatureByHour>(),
                        TemperatureMaxs = new List<TemperatureByHour>(),
                        TemperatureMins = new List<TemperatureByHour>()
                    };
                    listTemperatureTheoGioTmp = new List<TemperatureByHour>();
                }

            }
            duBaohietDoResponse.TemperatureByDays = listTemperatureTheoNgay;
            duBaohietDoResponse.TemperatureMin = listTemperatureTheoNgay.Min(x => x.TemperatureMins.Min(x => x.Temperature));
            duBaohietDoResponse.TemperatureMax = listTemperatureTheoNgay.Max(x => x.TemperatureMaxs.Max(x => x.Temperature));
            return duBaohietDoResponse;
        }
        
        public async Task<TemperatureResponse> GetTemperatureBy(string diemDuBaoId)
        {
            var nhietDoEntity = await _temperatureRepository.GetByIdAsync(diemDuBaoId);
            return _mapper.Map<TemperatureResponse>(nhietDoEntity);
        }
      
    }
}