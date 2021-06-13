using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using GloboWeather.WeatherManegement.Application.Models;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class RainRepository : BaseRepository<Rain>, IRainRepository
    {
        public RainRepository(
            MonitoringContext dbContext) : base(dbContext)
        {
           
        }

        public async Task<GetRainListResponse> GetByPagedAsync(GetRainsListQuery query)
        {
            var entryPoint = from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join rain in _dbContext.Set<Rain>() on tramKttv.StationId equals rain.StationId
                where query.ZipCodes.Contains(p.ZipCode) && (rain.Date >= query.DateFrom.Date && rain.Date <= query.DateTo.Date)
                orderby p.ZipCode descending
                select new RainListVm()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date = rain.Date,
                    RainQuantity = rain.Quality,
                    Address =  tramKttv.Address
                };
            var collection = await entryPoint.PaginateAsync(query.Page, query.Limit, new CancellationToken());
            return new GetRainListResponse()
            {
                CurrentPage = collection.CurrentPage,
                TotalPages = collection.TotalPages,
                TotalItems = collection.TotalItems,
                Rains = collection.Items.Select(r => new RainListVm
                {
                    ZipCode = r.ZipCode,
                    ProvinceName = r.ProvinceName,
                    StationName = r.StationName,
                    StationId = r.StationId,
                    Date = r.Date,
                    RainQuantity = r.RainQuantity,
                    Address =  r.Address
                }).ToList()
            };
            
        }
    }
}