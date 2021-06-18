using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
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

        public async Task<WeatherInformation> AddAsync(WeatherInformation information)
        {
            _unitOfWork.WeatherInformationRepository.Insert(information);
            await _unitOfWork.CommitAsync();
            return information;
        }

        public async Task ImportAsync(List<WeatherInformation> importData, CancellationToken token)
        {
            var maxRefDate = importData.Max(x => x.RefDate);
            var minRefDate = importData.Min(x => x.RefDate);
            var stationIds = importData.Select(x => x.StationId).Distinct().ToList();
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
    }
}