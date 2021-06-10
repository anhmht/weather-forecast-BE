using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
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
        
        public async Task<List<GetHydrologicalResponse>> GetHydrologicalAsync(IEnumerable<int> zipcodes)
        {
            var maxDate = _dbContext.Set<Hydrological>().Max(h => h.Date);
            
            var entryDatas = await (from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join  hydrological in _dbContext.Set<Hydrological>() on tramKttv.StationId equals hydrological.StationId
                where hydrological.Date.Equals(maxDate) && zipcodes.Contains(p.ZipCode)
                orderby p.ZipCode descending
                select new GetHydrologicalResponse()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date = hydrological.Date,
                    Rain = hydrological.Rain,
                    WaterLevel = hydrological.WaterLevel,
                    ZLuyKe =  hydrological.ZLuyKe
                }).ToListAsync();
            return entryDatas;
        }
    }
}