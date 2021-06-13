using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WindDirectionService : IWindDirectionService
    {
        private readonly IMapper _mapper;
        private readonly IHuongGioRepository _huongGioRepository;       

        public WindDirectionService(IMapper mapper,
            IHuongGioRepository huongGioRepository
            )
        {
            _mapper = mapper;
            _huongGioRepository = huongGioRepository;          
        }     
        /// <summary>
        /// get list of WindDirection by DiemId
        /// </summary>
        /// <param name="WindDirectionId"></param>
        /// <returns></returns>
        public async Task<WindDirectionPredictionResponse> GetWindDirectionByDiemId(string diemId)
        {
            var windDirectionEntity = await _huongGioRepository.GetByIdAsync(diemId);

            var windDirectionPredictionResponse = new WindDirectionPredictionResponse();
            if (windDirectionEntity == null)
                return new WindDirectionPredictionResponse();

            windDirectionPredictionResponse.DiemId = diemId;

            var currentDate = windDirectionEntity.RefDate;
            var windDirectionDayResponses = new List<WindDirectionDayResponse>();
            var windDirectionDayResponse = new WindDirectionDayResponse()
            {
                Date = currentDate,
                WindDirectionByHours = new List<WindDirectionHour>()
            };
            
            var windDirectionHourTmp = new List<WindDirectionHour>();
            int currentDay = 0;
            var windDirectionTimesMin = new List<WindDirectionTime>();
            var windDirectionTimeMax = new List<WindDirectionTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var WindDirection = windDirectionEntity.GetType().GetProperty($"_{i}").GetValue(windDirectionEntity, null);
                var WindDirectionTheoGio = new WindDirectionHour()
                {
                    Hour = nextHour.Hour,
                    WindDirection = (string)WindDirection
                };
                windDirectionHourTmp.Add(WindDirectionTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    windDirectionDayResponse.WindDirectionByHours.AddRange(windDirectionHourTmp);
                    // calculate WindDirection min or max
                    var WindDirectionMinTmp = windDirectionHourTmp.Min(x => x.WindDirection);
                    var WindDirectionMaxTmp = windDirectionHourTmp.Max(x => x.WindDirection);                  

                    windDirectionDayResponses.Add(windDirectionDayResponse);

                    // reinnit data
                    currentDay++;
                    windDirectionDayResponse = new WindDirectionDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        WindDirectionByHours = new List<WindDirectionHour>()
                       
                    };
                    windDirectionHourTmp = new List<WindDirectionHour>();
                }

            }
            windDirectionPredictionResponse.WindDirectionByDays = windDirectionDayResponses;        
            return windDirectionPredictionResponse;
        }


        public async Task<List<WindDirectionResponse>> ListAllAsync()
        {
            var data = await _huongGioRepository.ListAllAsync();
            var result = new List<WindDirectionResponse>();
            foreach (var item in data)
            {
                result.Add(_mapper.Map<WindDirectionResponse>(item));
            }
            return result;
        }
    }
}