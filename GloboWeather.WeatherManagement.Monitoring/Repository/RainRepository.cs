using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;

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
            var entryDatas = await _dbContext.Set<Rain>()
                .AsNoTracking()
                .Where(r => r.StationId.Equals(query.StationId)
                            && (r.Date >= query.DateFrom.Date && r.Date <= query.DateTo.Date))
                .PaginateAsync(query.Page, query.Limit, new CancellationToken());

            return new GetRainListResponse()
            {
                CurrentPage = entryDatas.CurrentPage,
                TotalPages = entryDatas.TotalPages,
                TotalItems = entryDatas.TotalPages,
                Rains = entryDatas.Items.Select(e => new RainListVm()
                {
                    Date = e.Date,
                    RainQuantity = e.Quality
                }).ToList()
            };
        }

        public async Task<List<Rain>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.Set<Rain>()
                .AsNoTracking()
                .Where(r => r.Date >= fromDate && r.Date <= toDate).ToListAsync();
        }
    }
}