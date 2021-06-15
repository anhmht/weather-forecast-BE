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
            var entryDatas = await _dbContext.Set<Hydrological>()
                .AsNoTracking()
                .Where(r => r.StationId.Equals(query.StationId)
                            && (r.Date >= query.DateFrom.Date && r.Date <= query.DateTo.Date))
                .PaginateAsync(query.Page, query.Limit, new CancellationToken());

            return new GetHydrologicalListResponse()
            {
                CurrentPage = entryDatas.CurrentPage,
                TotalItems = entryDatas.TotalItems,
                TotalPages = entryDatas.TotalPages,
                Hydrologicals = entryDatas.Items.Select(h => new HydrologicalListVm()
                {
                    Date = h.Date,
                    Rain = h.Rain,
                    WaterLevel = h.WaterLevel,
                    ZLuyKe = h.ZLuyKe,
                }).ToList()

            };
           
        }
    }
}