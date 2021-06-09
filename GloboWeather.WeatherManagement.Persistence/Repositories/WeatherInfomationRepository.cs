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
    public class WeatherInformationRepository: BaseRepository<WeatherInformation>, IWeatherInformationRepository
    {
        public WeatherInformationRepository(GloboWeatherDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> SaveAsync(List<WeatherInformation> WeatherInformations)
        {
            return true;
        }
        
    }
}