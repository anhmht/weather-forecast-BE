using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Weather;
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

        public async Task<bool> SaveAsync(List<WeatherInformation> WeatherInformations)
        {
            return true;
        }

        public async Task UpdateWinLevelAsync(List<WinLevelResponse> WeatherInformations, DateTime lastUpdate)
        {
            var predictData = _dbContext.WeatherInformations.Where(x => x.RefDate > lastUpdate).ToList();
            foreach (var item in WeatherInformations)
            {
                var currentDay = item.RefDate;
                var predictDataTmp = _dbContext.WeatherInformations.Where(x => x.RefDate > currentDay && x.StationId == item.DiemId);
                for (int i = 1; i < 121; i++)
                {
                    var predictTime = currentDay.AddHours(i);
                    var value = item.GetType().GetProperty($"_{i}").GetValue(item, null);

                    var winlevel = predictDataTmp.FirstOrDefault(x => x.RefDate == predictTime);
                    if (winlevel != null)
                    {
                        winlevel.WindLevel = (string)value;
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
            _dbContext.SaveChanges();
        }


    }
}