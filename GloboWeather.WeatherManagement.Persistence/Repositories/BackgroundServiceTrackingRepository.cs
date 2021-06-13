using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class BackgroundServiceTrackingRepository: BaseRepository<BackgroundServiceTracking>, IBackgroundServiceTrackingRepository
    {
        public BackgroundServiceTrackingRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<BackgroundServiceTracking> GetLastBackgroundServiceTracking()
        {
            var result = _dbContext.BackgroundServiceTrackings.OrderByDescending(x=>x.LastDownload).Take(1).ToList().FirstOrDefault();
            return result;
        }
    }
}