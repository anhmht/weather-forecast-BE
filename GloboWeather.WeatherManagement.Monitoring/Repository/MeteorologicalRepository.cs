using System;
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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class MeteorologicalRepository : BaseRepository<Meteorological>, IMeteorologicalRepository
    {
        public MeteorologicalRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }

        public async Task<GetMeteorologicalListResponse> GetByPagedAsync(GetMeteorologicalListQuery query)
        {
            var entryDatas = await _dbContext.Set<Meteorological>()
                .AsNoTracking()
                .Where(r => r.StationId.Equals(query.StationId)
                            && (r.Date >= query.DateFrom.Date && r.Date <= query.DateTo.Date))
                .PaginateAsync(query.Page, query.Limit, new CancellationToken());

            return new GetMeteorologicalListResponse()
            {
                CurrentPage = entryDatas.CurrentPage,
                TotalItems = entryDatas.TotalItems,
                TotalPages = entryDatas.TotalPages,
                Meteorologicals = entryDatas.Items.Select(m => new MeteorologicalListVm()
                {
                    Date = m.Date,
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
                    WindSpeed = m.WindSpeed,
                    ZluyKe = m.ZluyKe,
                }).ToList()
            };
        }

        public async Task<List<Meteorological>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.Set<Meteorological>()
                .AsNoTracking()
                .Where(r => r.Date >= fromDate && r.Date <= toDate).ToListAsync();
        }
    }
}