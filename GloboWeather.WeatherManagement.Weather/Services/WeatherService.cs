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
        private readonly INhietDoRepository _nhietDoRepository;

        public WeatherService(IMapper mapper,
            IDiemDuBaoRepository diemDuBaoRepository
            , INhietDoRepository nhietDoRepository)
        {
            _mapper = mapper;
            _diemDuBaoRepository = diemDuBaoRepository;
            _nhietDoRepository = nhietDoRepository;
        }
        public async Task<List<DiemDuBaoResponse>> GetDiemDuBaosList()
        {
            var diemdubaoEntityList = await _diemDuBaoRepository.ListAllAsync();

            return _mapper.Map<List<DiemDuBaoResponse>>(diemdubaoEntityList);

        }

        public async Task<NhietDoResponse> GetNhietDoBy(string diemDuBaoId)
        {
            var nhietDoEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);
            return _mapper.Map<NhietDoResponse>(nhietDoEntity);
        }

        /// <summary>
        /// get list of NhietDo by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<TemperaturePredictionResponse> GetNhietDoByDiemId(string diemDuBaoId)
        {
            var nhietDoEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new TemperaturePredictionResponse();
            if (nhietDoEntity == null)
                return new TemperaturePredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

            var currentDate = nhietDoEntity.RefDate;
            var listNhietDoTheoNgay = new List<TemperatureDayResponse>();
            var nhietDoTheoNgay = new TemperatureDayResponse()
            {
                Date = currentDate,
                TemperatureHours = new List<TemperatureHour>()
            };

            var listNhietDoTheoGioTmp = new List<TemperatureHour>();
            int currentDay = 0;
            var nhietDoTheoThoiGianMin = new List<TemperatureTime>();
            var nhietDoTheoThoiGianMax = new List<TemperatureTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var nhietDo = nhietDoEntity.GetType().GetProperty($"_{i}").GetValue(nhietDoEntity, null);
                var nhietDoTheoGio = new TemperatureHour()
                {
                    Hour = nextHour.Hour,
                    Temperature = (int)nhietDo
                };
                listNhietDoTheoGioTmp.Add(nhietDoTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    nhietDoTheoNgay.TemperatureHours.AddRange(listNhietDoTheoGioTmp);
                    // calculate temperature min or max
                    var nhietDoMinTmp = listNhietDoTheoGioTmp.Min(x => x.Temperature);
                    var nhietDoMaxTmp = listNhietDoTheoGioTmp.Max(x => x.Temperature);
                    nhietDoTheoNgay.TemperatureMins.AddRange(listNhietDoTheoGioTmp.Where(x => x.Temperature == nhietDoMinTmp));
                    nhietDoTheoNgay.TemperatureMaxs.AddRange(listNhietDoTheoGioTmp.Where(x => x.Temperature == nhietDoMaxTmp));
                    nhietDoTheoNgay.TemperatureMin = nhietDoMinTmp;
                    nhietDoTheoNgay.TemperatureMax = nhietDoMaxTmp;

                    listNhietDoTheoNgay.Add(nhietDoTheoNgay);
                                       
                    // reinnit data
                    currentDay++;
                    nhietDoTheoNgay = new TemperatureDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        TemperatureHours = new List<TemperatureHour>(),
                        TemperatureMaxs = new List<TemperatureHour>(),
                        TemperatureMins = new List<TemperatureHour>()
                    };
                    listNhietDoTheoGioTmp = new List<TemperatureHour>();
                }

            }
            duBaohietDoResponse.TemperatureDays = listNhietDoTheoNgay;
            duBaohietDoResponse.TemperatureMin = listNhietDoTheoNgay.Min(x=>x.TemperatureMins.Min(x=>x.Temperature));
            duBaohietDoResponse.TemperatureMax = listNhietDoTheoNgay.Max(x => x.TemperatureMaxs.Max(x => x.Temperature));
            return duBaohietDoResponse;
        }


        /// <summary>
        /// get list of WindSpeed by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<WindSpeedPredictionResponse> GetWindSpeedByDiemId(string diemDuBaoId)
        {
            var WindSpeedEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new WindSpeedPredictionResponse();
            if (WindSpeedEntity == null)
                return new WindSpeedPredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

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


        /// <summary>
        /// get list of Humidity by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<HumidityPredictionResponse> GetHumidityByDiemId(string diemDuBaoId)
        {
            var HumidityEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new HumidityPredictionResponse();
            if (HumidityEntity == null)
                return new HumidityPredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

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

        /// <summary>
        /// get list of RainAmount by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<RainAmountPredictionResponse> GetRainAmountByDiemId(string diemDuBaoId)
        {
            var RainAmountEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new RainAmountPredictionResponse();
            if (RainAmountEntity == null)
                return new RainAmountPredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

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

        /// <summary>
        /// get list of WindLevel by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<WindLevelPredictionResponse> GetWindLevelByDiemId(string diemDuBaoId)
        {
            var WindLevelEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new WindLevelPredictionResponse();
            if (WindLevelEntity == null)
                return new WindLevelPredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

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

        /// <summary>
        /// get list of Weather by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<WeatherPredictionResponse> GetWeatherByDiemId(string diemDuBaoId)
        {
            var WeatherEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

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
                    Weather = (int)Weather
                };
                listWeatherTheoGioTmp.Add(WeatherTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    WeatherTheoNgay.WeatherByHours.AddRange(listWeatherTheoGioTmp);
                    // calculate Weather min or max
                    var WeatherMinTmp = listWeatherTheoGioTmp.Min(x => x.Weather);
                    var WeatherMaxTmp = listWeatherTheoGioTmp.Max(x => x.Weather);
                    WeatherTheoNgay.WeatherMins.AddRange(listWeatherTheoGioTmp.Where(x => x.Weather == WeatherMinTmp));
                    WeatherTheoNgay.WeatherMaxs.AddRange(listWeatherTheoGioTmp.Where(x => x.Weather == WeatherMaxTmp));
                    WeatherTheoNgay.WeatherMin = WeatherMinTmp;
                    WeatherTheoNgay.WeatherMax = WeatherMaxTmp;

                    listWeatherTheoNgay.Add(WeatherTheoNgay);

                    // reinnit data
                    currentDay++;
                    WeatherTheoNgay = new WeatherDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        WeatherByHours = new List<WeatherHour>(),
                        WeatherMaxs = new List<WeatherHour>(),
                        WeatherMins = new List<WeatherHour>()
                    };
                    listWeatherTheoGioTmp = new List<WeatherHour>();
                }

            }
            duBaohietDoResponse.WeatherByDays = listWeatherTheoNgay;
            duBaohietDoResponse.WeatherMin = listWeatherTheoNgay.Min(x => x.WeatherMins.Min(x => x.Weather));
            duBaohietDoResponse.WeatherMax = listWeatherTheoNgay.Max(x => x.WeatherMaxs.Max(x => x.Weather));
            return duBaohietDoResponse;
        }
    }
}