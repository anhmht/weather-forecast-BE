using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class MeteorologicalRepository : BaseRepository<Meteorological>, IMeteorologicalRepository
    {
        public MeteorologicalRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<GetMeteorologicalResponse>> GetMeteotologicalsAsync()
        {
            var maxDate = _dbContext.Set<Meteorological>().Max(h => h.Date);
            
            var entryDatas = await (from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join  meteorological in _dbContext.Set<Meteorological>() on tramKttv.StationId equals meteorological.StationId
                where meteorological.Date.Equals(maxDate)
                orderby p.ZipCode descending
                select new GetMeteorologicalResponse()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date =  meteorological.Date,
                    Barometric = meteorological.Barometric,
                    Evaporation = meteorological.Evaporation,
                    Hga10 = meteorological.Hga10,
                    Hgm60 = meteorological.Hgm60,
                    Humidity = meteorological.Humidity,
                    Radiation = meteorological.Radiation,
                    Rain = meteorological.Rain,
                    Tdga10 = meteorological.Tdga10,
                    Tdgm60 = meteorological.Tdgm60,
                    Temperature = meteorological.Temperature,
                    WindDirection = meteorological.WindDirection,
                    WindSpeed =  meteorological.WindSpeed,
                    ZluyKe =  meteorological.ZluyKe
                }).ToListAsync();
            return entryDatas;
        }
    }
}