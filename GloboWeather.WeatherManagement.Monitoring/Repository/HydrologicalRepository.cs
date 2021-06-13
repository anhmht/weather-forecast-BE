using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class HydrologicalRepository : BaseRepository<Hydrological>, IHydrologicalRepository
    {
        public HydrologicalRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }
        
        public async Task<GetHydrologicalListResponse> GetByPagedAsync(GetHydrologicalListQuery query)
        {
            var entryDatas = from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join hydrological in _dbContext.Set<Hydrological>() on tramKttv.StationId equals hydrological.StationId
                where query.ZipCodes.Contains(p.ZipCode) && (hydrological.Date >= query.DateFrom && hydrological.Date <= query.DateTo)
                orderby p.ZipCode descending
                select new HydrologicalListVm()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date = hydrological.Date,
                    Rain = hydrological.Rain,
                    WaterLevel = hydrological.WaterLevel,
                    ZLuyKe = hydrological.ZLuyKe,
                    Address =  tramKttv.Address
                };
            
            var collection = await entryDatas.PaginateAsync(query.Page, query.Limit, new CancellationToken());
            return new GetHydrologicalListResponse()
            {
                CurrentPage = collection.CurrentPage,
                TotalPages = collection.TotalPages,
                TotalItems = collection.TotalItems,
                Hydrologicals = collection.Items.Select(h => new HydrologicalListVm()
                {
                    ZipCode = h.ZipCode,
                    ProvinceName = h.ProvinceName,
                    StationName = h.StationName,
                    StationId = h.StationId,
                    Date = h.Date,
                    Rain = h.Rain,
                    WaterLevel = h.WaterLevel,
                    ZLuyKe = h.ZLuyKe,
                    Address =  h.Address
                }).ToList()
            };
        }
    }
}