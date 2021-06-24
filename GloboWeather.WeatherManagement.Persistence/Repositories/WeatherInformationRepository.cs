using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class WeatherInformationRepository : BaseRepository<WeatherInformation>, IWeatherInformationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public WeatherInformationRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WeatherInformation>> GetByRefDateStationAsync(DateTime startDate, DateTime endDate, IEnumerable<string> stationIds, CancellationToken token)
        {
            if (stationIds?.Any() == true)
            {
                return (await _unitOfWork.WeatherInformationRepository.GetWhereAsync(x =>
                        x.RefDate <= endDate && x.RefDate >= startDate && stationIds.Contains(x.StationId)))
                    .OrderBy(x => x.RefDate);
            }
            else
            {
                return (await _unitOfWork.WeatherInformationRepository.GetWhereAsync(x =>
                        x.RefDate <= endDate && x.RefDate >= startDate))
                    .OrderBy(x => x.RefDate);
            }
        }

        #region Sync Data weather from my sql to sql

        /// <summary>
        /// sync data giogiat
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncWinLevelAsync(List<WinLevelResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = await _unitOfWork.WeatherInformationRepository.GetWhereAsync(x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindLevel = ((int)value).ToString();
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            WindLevel = ((int)value).ToString(),
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };

                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }

            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// sync data doamtb
        /// </summary>
        /// <param name="weatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <param name="isSaveDb"></param>
        /// <returns></returns>
        public async Task SyncHumidityAsync(List<HumidityResponse> weatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in weatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = await _unitOfWork.WeatherInformationRepository.GetWhereAsync(x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Humidity = ((int)value).ToString();
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            Humidity = ((int)value).ToString(),
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// sync data huonggio
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncWindDirectionAsync(List<WindDirectionResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = await _unitOfWork.WeatherInformationRepository.GetWhereAsync(x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindDirection = (string)value;
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            WindDirection = (string)value,
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// sync data tocdogio
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncWindSpeedAsync(List<WindSpeedResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _unitOfWork.WeatherInformationRepository.GetWhereAsync(
                        x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindSpeed = ((int)value).ToString();
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            WindSpeed = ((int)value).ToString(),
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// sync data nhietdo
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncTemperatureAsync(List<TemperatureResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _unitOfWork.WeatherInformationRepository.GetWhereAsync(
                        x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Temperature = ((int)value).ToString();
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            Temperature = ((int)value).ToString(),
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        public async Task SyncRainAmountAsync(List<RainAmountResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _unitOfWork.WeatherInformationRepository.GetWhereAsync(
                        x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.RainAmount = ((int)value).ToString();
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            RainAmount = ((int)value).ToString(),
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        public async Task SyncWeatherAsync(List<WeatherResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _unitOfWork.WeatherInformationRepository.GetWhereAsync(
                        x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}")?.GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Weather = (string)value;
                        weatherInformation.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var newWeatherInformation = new WeatherInformation()
                        {
                            CreateDate = DateTime.Now,
                            Weather = (string)value,
                            RefDate = predictTime,
                            StationId = item.DiemId,
                            CreateBy = "System"
                        };
                        _unitOfWork.WeatherInformationRepository.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _unitOfWork.CommitAsync();
        }

        #endregion

        public async Task ImportAsync(List<WeatherInformation> importData, CancellationToken token)
        {
            var maxRefDate = importData.Max(x => x.RefDate);
            var minRefDate = importData.Min(x => x.RefDate);
            var stationIds = importData.Select(x => x.StationId).Distinct().ToList();

            await Import(minRefDate, maxRefDate, stationIds, importData, token);
        }

        public async Task<GetWeatherInformationResponse> ImportSingleStationAsync(string stationId, string stationName, List<WeatherInformation> importData, CancellationToken token)
        {
            var station = await _unitOfWork.StationRepository.FindAsync(x => x.ID == stationId);
            if (station == null)
            {
                station = new Station()
                {
                    ID = stationId,
                    Name = stationName
                };
                _unitOfWork.StationRepository.Add(station);
            }

            importData.ForEach(item => item.StationId = stationId);
            var maxRefDate = importData.Max(x => x.RefDate);
            var minRefDate = importData.Min(x => x.RefDate);
            var stationIds = new List<string>() {stationId};

            await Import(minRefDate, maxRefDate, stationIds, importData, token);

            var getWeatherInformationRequest = new GetWeatherInformationRequest()
            {
                FromDate = minRefDate,
                StationIds = stationIds,
                ToDate = maxRefDate
            };

            RequestHelper.StandadizeGetWeatherInformationBaseRequest(getWeatherInformationRequest, false);
            return await GetWeatherInformationsAsync(getWeatherInformationRequest, token);
        }

        public async Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request
            , CancellationToken cancelToken)
        {
            var response = new GetWeatherInformationResponse();

            var weatherInformations = await GetByRefDateStationAsync(request.FromDate.Value, request.ToDate.Value, request.StationIds, cancelToken);
            if (weatherInformations?.Any() == false)
                return response;

            var stationIds = weatherInformations.Select(x => x.StationId).Distinct();
            var parallelOption = new ParallelOptions() { CancellationToken = cancelToken, MaxDegreeOfParallelism = 8 };

            var weatherInformationItems = new ConcurrentBag<WeatherInformationByStation>();
            Parallel.ForEach(stationIds, parallelOption, (stationId) =>
                //foreach (var stationId in stationIds)
            {
                var weatherInformationByStations = weatherInformations.Where(x => x.StationId == stationId);
                if (!weatherInformationByStations.Any())
                    return; // continue;

                foreach (var weatherType in request.WeatherTypes)
                {
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
                        var weatherInformationInDate = weatherInformationByStations
                            .Where(x => x.RefDate.Date == dateInterval.Value.Date).OrderBy(x => x.RefDate);
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

                            if (weatherType != WeatherType.WindDirection)
                            {
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
                        }

                        weatherInformationByStation.WeatherInformationByDays.Add(weatherInformationByDay);

                        dateInterval = dateInterval.Value.AddDays(1);
                    }

                    weatherInformationItems.Add(weatherInformationByStation);
                }
            });


            response.WeatherInformationByStations.AddRange(weatherInformationItems);
            return response;
        }


        public async Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(
            GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken)
        {
            var response = new GetWeatherInformationHorizontalResponse();

            var weatherInformations = await GetByRefDateStationAsync(request.FromDate.Value,
                request.ToDate.Value.AddDays(4), request.StationIds, cancelToken);
            if (weatherInformations?.Any() == false)
                return response;

            var weatherInformationHorizontals = new ConcurrentBag<GetWeatherInformationHorizontal>();
            var stationIds = weatherInformations.Select(x => x.StationId).Distinct();
            var parallelOption = new ParallelOptions() {CancellationToken = cancelToken, MaxDegreeOfParallelism = 8};
            Parallel.ForEach(stationIds, parallelOption, (stationId) =>
            {
                var weatherInformationByStations = weatherInformations.Where(x => x.StationId == stationId);
                if (!weatherInformationByStations.Any())
                    return; //continue;

                foreach (var weatherType in request.WeatherTypes)
                {
                    var dateInterval = request.FromDate.Value;
                    while (dateInterval <= request.ToDate)
                    {
                        var weatherInformationHorizontal = new GetWeatherInformationHorizontal()
                        {
                            StationId = stationId,
                            RefDate = dateInterval,
                            WeatherType = weatherType
                        };

                        for (int i = 0; i < 120; i++)
                        {
                            var weatherInformation =
                                weatherInformationByStations.SingleOrDefault(x =>
                                    x.RefDate == dateInterval.AddHours(i));
                            if (weatherInformation == null)
                                continue;
                            var value = GetValueByWeatherType(weatherInformation, weatherType);
                            var fieldName = $"_{i + 1}";
                            var propertyInfo = weatherInformationHorizontal.GetType().GetProperty(fieldName);
                            propertyInfo?.SetValue(weatherInformationHorizontal,
                                Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }

                        weatherInformationHorizontals.Add(weatherInformationHorizontal);

                        dateInterval = dateInterval.AddDays(1);
                    }
                }
            });
            response.GetWeatherInformationHorizontals.AddRange(weatherInformationHorizontals);
            return response;
        }

        #region Private functions

        private async Task Import(DateTime minRefDate, DateTime maxRefDate, IEnumerable<string> stationIds
            , List<WeatherInformation> importData, CancellationToken token)
        {
            var existingData =
                await GetByRefDateStationAsync(minRefDate, maxRefDate, stationIds, token);

            var listInsert = (from i in importData
                              join e in existingData on new { i.StationId, i.RefDate } equals new { e.StationId, e.RefDate }
                                  into t
                              from imp in t.DefaultIfEmpty()
                              where imp == null
                              select new WeatherInformation()
                              {
                                  //CreateBy = "import",
                                  //CreateDate = DateTime.Now,
                                  Humidity = i.Humidity,
                                  ID = Guid.NewGuid(),
                                  //LastModifiedBy = "import",
                                  //LastModifiedDate = DateTime.Now,
                                  RainAmount = i.RainAmount,
                                  RefDate = i.RefDate,
                                  StationId = i.StationId,
                                  Temperature = i.Temperature,
                                  Weather = i.Weather,
                                  WindDirection = i.WindDirection,
                                  WindLevel = i.WindLevel,
                                  WindSpeed = i.WindSpeed
                              }).ToList();

            if (listInsert.Count > 0)
                _unitOfWork.WeatherInformationRepository.AddRange(listInsert);

            if (existingData.Any())
            {
                foreach (var item in existingData)
                {
                    var updateItem = importData.FirstOrDefault(x => x.StationId == item.StationId && x.RefDate == item.RefDate);
                    if (updateItem != null)
                    {
                        if (!string.IsNullOrWhiteSpace(updateItem.Humidity)) //Don't update if don't input value for this field
                            item.Humidity = updateItem.Humidity;
                        if (!string.IsNullOrWhiteSpace(updateItem.RainAmount)
                        ) //Don't update if don't input value for this field
                            item.RainAmount = updateItem.RainAmount;
                        if (!string.IsNullOrWhiteSpace(updateItem.Temperature)
                        ) //Don't update if don't input value for this field
                            item.Temperature = updateItem.Temperature;
                        if (!string.IsNullOrWhiteSpace(updateItem.Weather)) //Don't update if don't input value for this field
                            item.Weather = updateItem.Weather;
                        if (!string.IsNullOrWhiteSpace(updateItem.WindDirection)
                        ) //Don't update if don't input value for this field
                            item.WindDirection = updateItem.WindDirection;
                        if (!string.IsNullOrWhiteSpace(updateItem.WindLevel)) //Don't update if don't input value for this field
                            item.WindLevel = updateItem.WindLevel;
                        if (!string.IsNullOrWhiteSpace(updateItem.WindSpeed)) //Don't update if don't input value for this field
                            item.WindSpeed = updateItem.WindSpeed;
                    }
                }

                _unitOfWork.WeatherInformationRepository.UpdateRange(existingData.ToList());
            }

            await _unitOfWork.CommitAsync();
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
                        return weatherInformation.Weather;
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
            if (weatherInformation != null && weatherInformation.Any())
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
                        return null;//weatherInformation.Select(x => x.Weather).Min();
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
            if (weatherInformation != null && weatherInformation.Any())
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
                        return null;//weatherInformation.Select(x => x.Weather).Max();
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

        #endregion


    }
}