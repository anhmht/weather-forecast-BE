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
    public class WeatherInfomationRepository: BaseRepository<WeatherInfomation>, IWeatherInfomationRepository
    {
        public WeatherInfomationRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> SaveAsync(List<WeatherInfomation> weatherInfomations)
        {
            return true;
        }
        
    }
}