using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using System.Linq;
using System.Threading;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class MeteorologicalRepository : BaseRepository<Meteorological>, IMeteorologicalRepository
    {
        public MeteorologicalRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }

        public async Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query)
        {
            var entryDatas = from p in _dbContext.Set<Province>()
                join tramKttv in _dbContext.Set<TramKttv>() on p.ZipCode equals tramKttv.ZipCode
                join meteorological in _dbContext.Set<Meteorological>() on tramKttv.StationId equals meteorological
                    .StationId
                where query.ZipCodes.Contains(p.ZipCode) && (meteorological.Date >= query.DateFrom.Date &&
                                                             meteorological.Date <= query.DateTo.Date)
                orderby p.ZipCode descending
                select new MeteorologicalListVm()
                {
                    ZipCode = p.ZipCode,
                    ProvinceName = p.Name,
                    StationName = tramKttv.Name,
                    StationId = tramKttv.StationId,
                    Date = meteorological.Date,
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
                    WindSpeed = meteorological.WindSpeed,
                    ZluyKe = meteorological.ZluyKe,
                    Address = tramKttv.Address
                };
            
            var collection = await entryDatas.PaginateAsync(query.Page, query.Limit, new CancellationToken());
            
            return new GetMeteorologicalListResponse()
            {
                CurrentPage = collection.CurrentPage,
                TotalPages = collection.TotalPages,
                TotalItems = collection.TotalItems,
                Meteorologicals = collection.Items.Select(m => new MeteorologicalListVm()
                {
                    ZipCode = m.ZipCode,
                    ProvinceName = m.ProvinceName,
                    StationName = m.StationName,
                    StationId = m.StationId,
                    Date =  m.Date,
                    Barometric = m.Barometric,
                    Evaporation = m.Evaporation,
                    Hga10 = m.Hga10,
                    Hgm60 = m.Hgm60,
                    Humidity = m.Humidity,
                    Radiation = m.Radiation,
                    Rain = m.Rain,
                    Tdga10 = m.Tdga10,
                    Tdgm60 = m.Tdgm60,
                    Temperature = m.Temperature,
                    WindDirection = m.WindDirection,
                    WindSpeed =  m.WindSpeed,
                    ZluyKe =  m.ZluyKe,
                    Address =  m.Address
                }).ToList()
            };
         
        }
    }
}