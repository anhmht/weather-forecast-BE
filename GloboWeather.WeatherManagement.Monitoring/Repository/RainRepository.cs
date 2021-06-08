using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class RainRepository : BaseRepository<Rain>, IRainRepository
    {
        public RainRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<GetRainMinMaxResponse>> GetMinMaxRain()
        {
            // var query = from provice in _dbContext.Set<Province>()
            //     join tramKttv in _dbContext.Set<TramKttv>() on provice.ZipCode equals tramKttv.ZipCode
            //     join rain in _dbContext.Set<Rain>() on tramKttv.StationId equals rain.StationId
            //     where tramKttv.StationType.Contains("mua")
            //     select new {provice.Name, rain.StationId};


            var sql =
                $"Select tinh_hq.ten as ProvinceName, DATE_FORMAT(m.dt, '%Y-%m-%d') as Date, MIN(m.mua) as Min, MAX(m.mua) as Max " +
                $"FROM tinh_hq " +
                $"JOIN tramkttv t on tinh_hq.matinhVN = t.MaTinh " +
                $"JOIN mua m on t.stationid = m.stationid " +
                $"where t.LoaiTram like '%mua%' and DATE_FORMAT(m.dt, '%Y-%m-%d') = '2021-05-20' " +
                $"GROUP BY tinh_hq.ten, DATE_FORMAT(m.dt, '%Y-%m-%d')";
            
             var entity = await _dbContext.Set<Rain>().FromSqlRaw(sql).ToListAsync();
            return new List<GetRainMinMaxResponse>();
        }
    }
}