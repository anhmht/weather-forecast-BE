using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WeatherInformationService : IWeatherInformationService
    {
        private readonly IMapper _mapper;
        private readonly IWeatherInformationRepository _weatherInformationRepository;

        public WeatherInformationService(IMapper mapper,
            IWeatherInformationRepository weatherInformationRepository
            )
        {
            _mapper = mapper;
            _weatherInformationRepository = weatherInformationRepository;
        }

        public async Task<HumidityPredictionResponse> GetHumidityAsync(string stationId, DateTime fromDate, DateTime toDate, CancellationToken cancelToken)
        {
            var response = new HumidityPredictionResponse()
            {
                DiemId = stationId,
                HumidityByDays = new List<HumidityDayResponse>()
            };
            fromDate = fromDate.GetStartOfDate();
            toDate = toDate.GetEndOfDate();
            var weatherInformations = await _weatherInformationRepository.GetByRefDateStationAsync(fromDate, toDate, new List<string>() { stationId }, cancelToken);
            if (weatherInformations?.Any() == false)
                return response;

            response.HumidityMin = weatherInformations.Min(x => x.Humidity).GetInt();
            response.HumidityMax = weatherInformations.Max(x => x.Humidity).GetInt();

            response.HumidityByDays = new List<HumidityDayResponse>();

            var dateInterval = fromDate;
            while (dateInterval <= toDate)
            {
                var weatherInformationInDate = weatherInformations.Where(x => x.RefDate.Date == dateInterval.Date).OrderBy(x => x.RefDate);
                var humidityDayResponse = new HumidityDayResponse()
                {
                    Date = dateInterval,
                    HumidityByHours = new List<HumidityHour>(),
                    HumidityMax = weatherInformationInDate.Max(x => x.Humidity).GetInt(),
                    HumidityMaxs = new List<HumidityHour>(),
                    HumidityMin = weatherInformationInDate.Min(x => x.Humidity).GetInt(),
                    HumidityMins = new List<HumidityHour>()
                };

                foreach (var weatherInfo in weatherInformationInDate)
                {
                    humidityDayResponse.HumidityByHours.Add(new HumidityHour()
                    {
                        Hour = weatherInfo.RefDate.Hour,
                        Humidity = weatherInfo.Humidity.GetInt()
                    });

                    if (weatherInfo.Humidity.GetInt() == humidityDayResponse.HumidityMin)
                    {
                        humidityDayResponse.HumidityMins.Add(new HumidityHour()
                        {
                            Hour = weatherInfo.RefDate.Hour,
                            Humidity = weatherInfo.Humidity.GetInt()
                        });
                    }

                    if (weatherInfo.Humidity.GetInt() == humidityDayResponse.HumidityMax)
                    {
                        humidityDayResponse.HumidityMaxs.Add(new HumidityHour()
                        {
                            Hour = weatherInfo.RefDate.Hour,
                            Humidity = weatherInfo.Humidity.GetInt()
                        });
                    }
                }

                response.HumidityByDays.Add(humidityDayResponse);

                dateInterval = dateInterval.AddDays(1);
            }

            return response;
        }

        public async Task<T> GetWeatherInformationVerticalAsync<T>(string stationId, DateTime fromDate, DateTime toDate, WeatherType weatherType, CancellationToken cancelToken)
        {
            var weatherInformations = (await GetWeatherInformationsAsync(stationId, fromDate, toDate, cancelToken)).ToList();

            var baseModelWeather = new BaseModelWeather()
            {
                DiemId = stationId,
                RefDate = fromDate,
            };

            var startDate = fromDate.GetStartOfDate();
            for (int i = 0; i < 120; i++)
            {
                var weatherInformation = weatherInformations.SingleOrDefault(x => x.RefDate == startDate.AddHours(i + 1));
                if (weatherInformation == null)
                    continue;

                var value = GetValueByWeatherType(weatherInformation, weatherType);
                var fieldName = $"_{i + 1}";
                var propertyInfo = baseModelWeather.GetType().GetProperty(fieldName);
                propertyInfo?.SetValue(baseModelWeather, Convert.ChangeType(value, Nullable.GetUnderlyingType(propertyInfo.PropertyType)), null);
            }

            return _mapper.Map<T>(baseModelWeather);
        }

        private async Task<IEnumerable<WeatherInformation>> GetWeatherInformationsAsync(string stationId, DateTime fromDate, DateTime toDate, CancellationToken cancelToken)
        {
            fromDate = fromDate.GetStartOfDate();
            toDate = toDate.GetEndOfDate();
            return await _weatherInformationRepository.GetByRefDateStationAsync(fromDate, toDate, new List<string>() { stationId }, cancelToken);
        }

        private object GetValueByWeatherType(WeatherInformation weatherInformation, WeatherType weatherType)
        {
            if (weatherInformation != null)
            {
                switch (weatherType)
                {
                    case WeatherType.Humidity:
                        return weatherInformation.Humidity.GetInt();
                    case WeatherType.RainAmount:
                        return weatherInformation.RainAmount.GetInt();
                    case WeatherType.Temperature:
                        return weatherInformation.Temperature.GetInt();
                    case WeatherType.Weather:
                        return weatherInformation.Weather.GetInt();
                    case WeatherType.WindDirection:
                        return weatherInformation.WindDirection;
                    case WeatherType.WindLevel:
                        return weatherInformation.WindLevel.GetInt();
                    case WeatherType.WindSpeed:
                        return weatherInformation.WindSpeed.GetInt();
                }
            }

            return null;
        }

        private int? GetMinValueByWeatherType(IEnumerable<WeatherInformation> weatherInformation, WeatherType weatherType)
        {
            if (weatherInformation != null)
            {
                switch (weatherType)
                {
                    case WeatherType.Humidity:
                        return weatherInformation.Select(x => x.Humidity.GetInt()).Min();
                    case WeatherType.RainAmount:
                        return weatherInformation.Select(x => x.RainAmount.GetInt()).Min();
                    case WeatherType.Temperature:
                        return weatherInformation.Select(x => x.Temperature.GetInt()).Min();
                    case WeatherType.Weather:
                        return weatherInformation.Select(x => x.Weather.GetInt()).Min();
                    case WeatherType.WindDirection:
                        return null;// weatherInformation.Select(x => x.WindDirection).Min();
                    case WeatherType.WindLevel:
                        return weatherInformation.Select(x => x.WindLevel.GetInt()).Min();
                    case WeatherType.WindSpeed:
                        return weatherInformation.Select(x => x.WindSpeed.GetInt()).Min();
                }
            }

            return null;
        }

        private int? GetMaxValueByWeatherType(IEnumerable<WeatherInformation> weatherInformation, WeatherType weatherType)
        {
            if (weatherInformation != null)
            {
                switch (weatherType)
                {
                    case WeatherType.Humidity:
                        return weatherInformation.Select(x => x.Humidity.GetInt()).Max();
                    case WeatherType.RainAmount:
                        return weatherInformation.Select(x => x.RainAmount.GetInt()).Max();
                    case WeatherType.Temperature:
                        return weatherInformation.Select(x => x.Temperature.GetInt()).Max();
                    case WeatherType.Weather:
                        return weatherInformation.Select(x => x.Weather.GetInt()).Max();
                    case WeatherType.WindDirection:
                        return null;// weatherInformation.Select(x => x.WindDirection).Max();
                    case WeatherType.WindLevel:
                        return weatherInformation.Select(x => x.WindLevel.GetInt()).Max();
                    case WeatherType.WindSpeed:
                        return weatherInformation.Select(x => x.WindSpeed.GetInt()).Max();
                }
            }

            return null;
        }

        public async Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken)
        {
            StandadizeGetWeatherInformationRequest(request);
            var response = new GetWeatherInformationResponse();

            var weatherInformations = await _weatherInformationRepository.GetByRefDateStationAsync(request.FromDate.Value, request.ToDate.Value, request.StationIds, cancelToken);
            if (weatherInformations?.Any() == false)
                return response;

            var stationIds = weatherInformations.Select(x => x.StationId).Distinct();
            foreach (var weatherType in request.WeatherTypes)
            {
                foreach (var stationId in stationIds)
                {
                    var weatherInformationByStations = weatherInformations.Where(x => x.StationId == stationId);

                    var weatherInformationByStation = new WeatherInformationByStation()
                    {
                        StationId = stationId,
                        WeatherType = weatherType,
                        MinValue = GetMinValueByWeatherType(weatherInformationByStations, weatherType),
                        MaxValue = GetMaxValueByWeatherType(weatherInformationByStations, weatherType)
                    };

                    var dateInterval = request.FromDate;
                    while (dateInterval <= request.ToDate)
                    {
                        var weatherInformationInDate = weatherInformationByStations.Where(x => x.RefDate.Date == dateInterval.Value.Date).OrderBy(x => x.RefDate);
                        var weatherInformationByDay = new WeatherInformationByDay()
                        {
                            Date = dateInterval.Value,
                            MinValue = GetMinValueByWeatherType(weatherInformationInDate, weatherType).GetInt(),
                            MaxValue = GetMaxValueByWeatherType(weatherInformationInDate, weatherType).GetInt()
                        };

                        foreach (var weatherInfo in weatherInformationInDate)
                        {
                            var value = GetValueByWeatherType(weatherInfo, weatherType);
                            weatherInformationByDay.WeatherInformationByHours.Add(new WeatherInformationByHour()
                            {
                                Hour = weatherInfo.RefDate.Hour,
                                Value = value
                            });

                            if (value.GetInt() == weatherInformationByDay.MinValue)
                            {
                                weatherInformationByDay.WeatherInformationMins.Add(new WeatherInformationByHour()
                                {
                                    Hour = weatherInfo.RefDate.Hour,
                                    Value = value
                                });
                            }

                            if (value.GetInt() == weatherInformationByDay.MaxValue)
                            {
                                weatherInformationByDay.WeatherInformationMaxs.Add(new WeatherInformationByHour()
                                {
                                    Hour = weatherInfo.RefDate.Hour,
                                    Value = value
                                });
                            }
                        }

                        weatherInformationByStation.WeatherInformationByDays.Add(weatherInformationByDay);

                        dateInterval = dateInterval.Value.AddDays(1);
                    }

                    response.WeatherInformationByStations.Add(weatherInformationByStation);
                }
            }

            return response;
        }

        private void StandadizeGetWeatherInformationRequest(GetWeatherInformationRequest request)
        {
            if (!request.FromDate.HasValue)
            {
                if (!request.ToDate.HasValue)
                {
                    request.FromDate = DateTime.Now.GetStartOfDate();
                    request.ToDate = DateTime.Now.GetEndOfDate();
                }
                else
                {
                    request.FromDate = request.ToDate.GetStartOfDate();
                    request.ToDate = request.ToDate.GetEndOfDate();
                }
            }
            else
            {
                request.FromDate = request.FromDate.GetStartOfDate();
                if (!request.ToDate.HasValue)
                {
                    request.ToDate = request.FromDate.GetEndOfDate();
                }
                else
                {
                    request.ToDate = request.ToDate.GetEndOfDate();
                }
            }

            if (request.FromDate > request.ToDate)
            {
                var dateTemp = request.FromDate;
                request.FromDate = request.ToDate;
                request.ToDate = dateTemp;
            }

            if (request.WeatherTypes?.Any() == false)
            {
                request.WeatherTypes = Enum.GetValues<WeatherType>();
            }
        }
    }
}