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
    public class RainAmountService : IRainAmountService
    {
        private readonly IMapper _mapper;
        private readonly IRainAmountRepository _rainAmountRepository;       

        public RainAmountService(IMapper mapper,
            IRainAmountRepository rainAmountRepository
            )
        {
            _mapper = mapper;
            _rainAmountRepository = rainAmountRepository;          
        }     
        /// <summary>
        /// get list of RainAmount by DiemId
        /// </summary>
        /// <param name="RainAmountId"></param>
        /// <returns></returns>
        public async Task<RainAmountPredictionResponse> GetRainAmountMinMaxByDiemId(string diemId)
        {
            var RainAmountEntity = await _rainAmountRepository.GetByIdAsync(diemId);

            var duBaohietDoResponse = new RainAmountPredictionResponse();
            if (RainAmountEntity == null)
                return new RainAmountPredictionResponse();

            duBaohietDoResponse.DiemId = diemId;

            var currentDate = RainAmountEntity.RefDate;
            var listRainAmountTheoNgay = new List<RainAmountDayResponse>();
            var RainAmountTheoNgay = new RainAmountDayResponse()
            {
                Date = currentDate,
                RainAmountByHours = new List<RainAmountHour>()
            };

            var listRainAmountTheoGioTmp = new List<RainAmountHour>();
            int currentDay = 0;
            var RainAmountTheoThoiGianMin = new List<RainAmountTime>();
            var RainAmountTheoThoiGianMax = new List<RainAmountTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var RainAmount = RainAmountEntity.GetType().GetProperty($"_{i}").GetValue(RainAmountEntity, null);
                var RainAmountTheoGio = new RainAmountHour()
                {
                    Hour = nextHour.Hour,
                    RainAmount = (int)RainAmount
                };
                listRainAmountTheoGioTmp.Add(RainAmountTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    RainAmountTheoNgay.RainAmountByHours.AddRange(listRainAmountTheoGioTmp);
                    // calculate RainAmount min or max
                    var RainAmountMinTmp = listRainAmountTheoGioTmp.Min(x => x.RainAmount);
                    var RainAmountMaxTmp = listRainAmountTheoGioTmp.Max(x => x.RainAmount);
                    RainAmountTheoNgay.RainAmountMins.AddRange(listRainAmountTheoGioTmp.Where(x => x.RainAmount == RainAmountMinTmp));
                    RainAmountTheoNgay.RainAmountMaxs.AddRange(listRainAmountTheoGioTmp.Where(x => x.RainAmount == RainAmountMaxTmp));
                    RainAmountTheoNgay.RainAmountMin = RainAmountMinTmp;
                    RainAmountTheoNgay.RainAmountMax = RainAmountMaxTmp;

                    listRainAmountTheoNgay.Add(RainAmountTheoNgay);

                    // reinnit data
                    currentDay++;
                    RainAmountTheoNgay = new RainAmountDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        RainAmountByHours = new List<RainAmountHour>(),
                        RainAmountMaxs = new List<RainAmountHour>(),
                        RainAmountMins = new List<RainAmountHour>()
                    };
                    listRainAmountTheoGioTmp = new List<RainAmountHour>();
                }

            }
            duBaohietDoResponse.RainAmountByDays = listRainAmountTheoNgay;
            duBaohietDoResponse.RainAmountMin = listRainAmountTheoNgay.Min(x => x.RainAmountMins.Min(x => x.RainAmount));
            duBaohietDoResponse.RainAmountMax = listRainAmountTheoNgay.Max(x => x.RainAmountMaxs.Max(x => x.RainAmount));
            return duBaohietDoResponse;
        }

        public async Task<AmountOfRainResponse> GetAmountOfRainBy(string diemDuBaoId)
        {
            var rainEntity = await _rainAmountRepository.GetByIdAsync(diemDuBaoId);

            return _mapper.Map<AmountOfRainResponse>(rainEntity);
        }
      
    }
}