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
        private readonly IUnitOfWork _;
        public WeatherInformationRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _ = unitOfWork;
        }

        public async Task<IEnumerable<WeatherInformation>> GetByRefDateStationAsync(DateTime startDate, DateTime endDate, IEnumerable<string> stationIds, CancellationToken token)
        {
            if (stationIds?.Any() == true)
            {
                return (await _.WeatherInformationRepository.Where(x =>
                        x.RefDate <= endDate && x.RefDate >= startDate && stationIds.Contains(x.StationId)))
                    .OrderBy(x => x.RefDate);
            }
            else
            {
                return (await _.WeatherInformationRepository.Where(x =>
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
                var predictDataTmp = await _.WeatherInformationRepository.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId);
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }

            if (isSaveDb)
                await _.CommitAsync();
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
                var predictDataTmp = await _.WeatherInformationRepository.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId);
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
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
                var predictDataTmp = await _.WeatherInformationRepository.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId);
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
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
                    await _.WeatherInformationRepository.Where(
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
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
                    await _.WeatherInformationRepository.Where(
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
        }

        public async Task SyncRainAmountAsync(List<RainAmountResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _.WeatherInformationRepository.Where(
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
        }

        public async Task SyncWeatherAsync(List<WeatherResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false)
        {
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp =
                    await _.WeatherInformationRepository.Where(
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
                        await _.WeatherInformationRepository.AddAsync(newWeatherInformation);
                    }
                }

            }
            if (isSaveDb)
                await _.CommitAsync();
        }

        #endregion

    }
}