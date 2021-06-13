using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
                        return weatherInformation.RainAmount;
                    case WeatherType.Temperature:
                        return weatherInformation.Temperature;
                    case WeatherType.Weather:
                        return weatherInformation.Weather;
                    case WeatherType.WindDirection:
                        return weatherInformation.WindDirection;
                    case WeatherType.WindLevel:
                        return weatherInformation.WindLevel;
                    case WeatherType.WindSpeed:
                        return weatherInformation.WindSpeed;
                }
            }

            return null;
        }
    }
}