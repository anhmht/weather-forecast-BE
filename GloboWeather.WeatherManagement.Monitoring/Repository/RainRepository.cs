using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class RainRepository : BaseRepository<Rain>, IRainRepository
    {
        public RainRepository(
            MonitoringContext dbContext) : base(dbContext)
        {
           
        }

        public async Task<List<GetRainResponse>> GetRainQuantityAsync(IEnumerable<int> zipcodes)
        {
            var maxDate = _dbContext.Set<Rain>().Max(r => r.Date);

            var entryPoint = await (from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join rain in _dbContext.Set<Rain>() on tramKttv.StationId equals rain.StationId
                where rain.Date.Equals(maxDate) && zipcodes.Contains(p.ZipCode)
                orderby p.ZipCode descending
                select new GetRainResponse()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date = rain.Date,
                    RainQuantity = rain.Quality
                }).ToListAsync();
            return entryPoint;
        }
    }
}