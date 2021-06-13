using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WindLevelService : IWindLevelService
    {
        private readonly IMapper _mapper;
        private readonly IWindLevelRepository _windLevelRepository;
        
        public WindLevelService(IMapper mapper,
            IWindLevelRepository windLevelRepository)
        {
            _mapper = mapper;
            _windLevelRepository = windLevelRepository;            
        }     
        /// <summary>
        /// get list of WindLevel by DiemId
        /// </summary>
        /// <param name="WindLevelId"></param>
        /// <returns></returns>
        public async Task<WindLevelPredictionResponse> GetWindLevelMinMaxByDiemId(string diemId)
        {
            var WindLevelEntity = await _windLevelRepository.GetByIdAsync(diemId);

            var duBaohietDoResponse = new WindLevelPredictionResponse();
            if (WindLevelEntity == null)
                return new WindLevelPredictionResponse();

            duBaohietDoResponse.DiemId = diemId;

            var currentDate = WindLevelEntity.RefDate;
            var listWindLevelTheoNgay = new List<WindLevelDayResponse>();
            var WindLevelTheoNgay = new WindLevelDayResponse()
            {
                Date = currentDate,
                WindLevelByHours = new List<WindLevelHour>()
            };

            var listWindLevelTheoGioTmp = new List<WindLevelHour>();
            int currentDay = 0;
            var WindLevelTheoThoiGianMin = new List<WindLevelTime>();
            var WindLevelTheoThoiGianMax = new List<WindLevelTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var WindLevel = WindLevelEntity.GetType().GetProperty($"_{i}").GetValue(WindLevelEntity, null);
                var WindLevelTheoGio = new WindLevelHour()
                {
                    Hour = nextHour.Hour,
                    WindLevel = (int)WindLevel
                };
                listWindLevelTheoGioTmp.Add(WindLevelTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    WindLevelTheoNgay.WindLevelByHours.AddRange(listWindLevelTheoGioTmp);
                    // calculate WindLevel min or max
                    var WindLevelMinTmp = listWindLevelTheoGioTmp.Min(x => x.WindLevel);
                    var WindLevelMaxTmp = listWindLevelTheoGioTmp.Max(x => x.WindLevel);
                    WindLevelTheoNgay.WindLevelMins.AddRange(listWindLevelTheoGioTmp.Where(x => x.WindLevel == WindLevelMinTmp));
                    WindLevelTheoNgay.WindLevelMaxs.AddRange(listWindLevelTheoGioTmp.Where(x => x.WindLevel == WindLevelMaxTmp));
                    WindLevelTheoNgay.WindLevelMin = WindLevelMinTmp;
                    WindLevelTheoNgay.WindLevelMax = WindLevelMaxTmp;

                    listWindLevelTheoNgay.Add(WindLevelTheoNgay);

                    // reinnit data
                    currentDay++;
                    WindLevelTheoNgay = new WindLevelDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        WindLevelByHours = new List<WindLevelHour>(),
                        WindLevelMaxs = new List<WindLevelHour>(),
                        WindLevelMins = new List<WindLevelHour>()
                    };
                    listWindLevelTheoGioTmp = new List<WindLevelHour>();
                }

            }
            duBaohietDoResponse.WindLevelByDays = listWindLevelTheoNgay;
            duBaohietDoResponse.WindLevelMin = listWindLevelTheoNgay.Min(x => x.WindLevelMins.Min(x => x.WindLevel));
            duBaohietDoResponse.WindLevelMax = listWindLevelTheoNgay.Max(x => x.WindLevelMaxs.Max(x => x.WindLevel));
            return duBaohietDoResponse;
        }

        public async Task<WindLevelResponse> GetWindLevelBy(string diemdubaoId)
        {
            var windLevelEntity = await _windLevelRepository.GetByIdAsync(diemdubaoId);
            
            return _mapper.Map<WindLevelResponse>(windLevelEntity);
        }

        public async Task<List<WinLevelResponse>> ListAllAsync()
        {
            var data  = await _windLevelRepository.ListAllAsync();
            var result = new List<WinLevelResponse>();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<WinLevelResponse>(item));
            }
            return result;
        }
    }
}