using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class WeatherInformationRepository : BaseRepository<WeatherInformation>, IWeatherInformationRepository
    {
        public WeatherInformationRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<WeatherInformation>> GetByRefDateStationAsync(DateTime startDate, DateTime endDate, IEnumerable<string> stationIds, CancellationToken token)
        {
            if (stationIds?.Any() == true)
                return await _dbContext.WeatherInformations.Where(x => x.RefDate <= endDate && x.RefDate >= startDate && stationIds.Contains(x.StationId)).OrderBy(x => x.RefDate).ToListAsync(token);
            else
                return await _dbContext.WeatherInformations.Where(x => x.RefDate <= endDate && x.RefDate >= startDate).OrderBy(x => x.RefDate).ToListAsync(token);
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
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindLevel = ((int)value).ToString();
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        /// <summary>
        /// sync data doamtb
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncHumidityAsync(List<HumidityResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Humidity = ((int)value).ToString();
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        /// <summary>
        /// sync data huonggio
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncWindDirectionAsync(List<WindDirectionResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindDirection = (string)value;
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        /// <summary>
        /// sync data tocdogio
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncWindSpeedAsync(List<WindSpeedResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.WindSpeed = ((int)value).ToString();
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        /// <summary>
        /// sync data nhietdo
        /// </summary>
        /// <param name="WeatherInformations"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public async Task SyncTemperatureAsync(List<TemperatureResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Temperature = ((int)value).ToString();
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        public async Task SyncRainAmountAsync(List<RainAmountResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.RainAmount = ((int)value).ToString();
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        public async Task SyncWeatherAsync(List<WeatherResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId).ToList();
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var weatherInformation = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (weatherInformation != null)
                    {
                        weatherInformation.Weather = (string)value;
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
                        _dbContext.WeatherInformations.Add(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                _dbContext.SaveChanges();
        }

        #endregion

    }
}