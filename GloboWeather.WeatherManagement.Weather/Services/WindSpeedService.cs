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
    public class WindSpeedService : IWindSpeedService
    {
        private readonly IMapper _mapper;
        private readonly ITocDoGioRepository _windSpeedRepository;       

        public WindSpeedService(IMapper mapper,
            ITocDoGioRepository windSpeedRepository
            )
        {
            _mapper = mapper;
            _windSpeedRepository = windSpeedRepository;          
        }     
        /// <summary>
        /// get list of WindSpeed by DiemId
        /// </summary>
        /// <param name="WindSpeedId"></param>
        /// <returns></returns>
        public async Task<WindSpeedPredictionResponse> GetWindSpeedByDiemId(string diemId)
        {
            var WindSpeedEntity = await _windSpeedRepository.GetByIdAsync(diemId);

            var duBaohietDoResponse = new WindSpeedPredictionResponse();
            if (WindSpeedEntity == null)
                return new WindSpeedPredictionResponse();

            duBaohietDoResponse.DiemId = diemId;

            var currentDate = WindSpeedEntity.RefDate;
            var listWindSpeedTheoNgay = new List<WindSpeedDayResponse>();
            var WindSpeedTheoNgay = new WindSpeedDayResponse()
            {
                Date = currentDate,
                WindSpeedByHours = new List<WindSpeedHour>()
            };

            var listWindSpeedTheoGioTmp = new List<WindSpeedHour>();
            int currentDay = 0;
            var WindSpeedTheoThoiGianMin = new List<WindSpeedTime>();
            var WindSpeedTheoThoiGianMax = new List<WindSpeedTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var WindSpeed = WindSpeedEntity.GetType().GetProperty($"_{i}").GetValue(WindSpeedEntity, null);
                var WindSpeedTheoGio = new WindSpeedHour()
                {
                    Hour = nextHour.Hour,
                    WindSpeed = (int)WindSpeed
                };
                listWindSpeedTheoGioTmp.Add(WindSpeedTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    WindSpeedTheoNgay.WindSpeedByHours.AddRange(listWindSpeedTheoGioTmp);
                    // calculate WindSpeed min or max
                    var WindSpeedMinTmp = listWindSpeedTheoGioTmp.Min(x => x.WindSpeed);
                    var WindSpeedMaxTmp = listWindSpeedTheoGioTmp.Max(x => x.WindSpeed);
                    WindSpeedTheoNgay.WindSpeedMins.AddRange(listWindSpeedTheoGioTmp.Where(x => x.WindSpeed == WindSpeedMinTmp));
                    WindSpeedTheoNgay.WindSpeedMaxs.AddRange(listWindSpeedTheoGioTmp.Where(x => x.WindSpeed == WindSpeedMaxTmp));
                    WindSpeedTheoNgay.WindSpeedMin = WindSpeedMinTmp;
                    WindSpeedTheoNgay.WindSpeedMax = WindSpeedMaxTmp;

                    listWindSpeedTheoNgay.Add(WindSpeedTheoNgay);

                    // reinnit data
                    currentDay++;
                    WindSpeedTheoNgay = new WindSpeedDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        WindSpeedByHours = new List<WindSpeedHour>(),
                        WindSpeedMaxs = new List<WindSpeedHour>(),
                        WindSpeedMins = new List<WindSpeedHour>()
                    };
                    listWindSpeedTheoGioTmp = new List<WindSpeedHour>();
                }

            }
            duBaohietDoResponse.WindSpeedByDays = listWindSpeedTheoNgay;
            duBaohietDoResponse.WindSpeedMin = listWindSpeedTheoNgay.Min(x => x.WindSpeedMins.Min(x => x.WindSpeed));
            duBaohietDoResponse.WindSpeedMax = listWindSpeedTheoNgay.Max(x => x.WindSpeedMaxs.Max(x => x.WindSpeed));
            return duBaohietDoResponse;
        }
      
    }
}