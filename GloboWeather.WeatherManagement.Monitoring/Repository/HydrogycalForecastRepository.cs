using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class HydrologicalForecastRepository : BaseRepository<HydrologicalForecast>, IHydrologicalForecastRepository
    {
        public HydrologicalForecastRepository(
            MonitoringContext dbContext) : base(dbContext)
        {
        }

        public async Task<GetHydrologicalForecastListResponse> GetByPagedAsync(GetHydrologicalForecastListQuery query)
        {
            var dateFrom = query.DateFrom.GetStartOfDate();
            var dateTo = query.DateTo.GetEndOfDate();
            var entryDatas = await _dbContext.Set<HydrologicalForecast>()
                .AsNoTracking()
                .Where(r => query.StationIds.Contains(r.StationId)
                            && (r.RefDate >= dateFrom && r.RefDate <= dateTo)).OrderBy(x=>x.RefDate).ToListAsync();
            //.Paginate(query.Page, query.Limit, new CancellationToken());

            var response = new GetHydrologicalForecastListResponse();
            var intervalDate = dateFrom;
            while (intervalDate < dateTo)
            {
                foreach (var stationId in query.StationIds)
                {
                    var stationDataInDay = entryDatas.Where(x => x.RefDate.Date == intervalDate && x.StationId == stationId);
                    var hydrologicalForecast = new HydrologicalForecastListVm { StationId = stationId, RefDate = intervalDate };

                    var minRow = stationDataInDay.FirstOrDefault(x => x.MinMax.ToLower() == "min");
                    var maxRow = stationDataInDay.FirstOrDefault(x => x.MinMax.ToLower() == "max");

                    for (var i = 1; i <= 5; i++)
                    {
                        var min = GetValueByDay(minRow, i);
                        var max = GetValueByDay(maxRow, i);
                        hydrologicalForecast.Data.Add(new HydrologicalForecastVm
                        {
                            Date = intervalDate.AddDays(i),
                            Max = max,
                            Min = min,
                            Value = max == null && min == null ? null : $"{min.GetString("--")}m - {max.GetString("--")}m"
                        });
                    }

                    response.GetHydrologicalForecasts.Add(hydrologicalForecast);
                }

                intervalDate = intervalDate.AddDays(1);
            }

            var paging = response.GetHydrologicalForecasts.Paginate(query.Page, query.Limit);
            response.CurrentPage = paging.CurrentPage;
            response.TotalPages = paging.TotalPages;
            response.TotalItems = paging.TotalItems;
            
            return response;
        }

        private float? GetValueByDay(HydrologicalForecast entry, int day)
        {
            if (entry == null)
                return null;
            switch (day)
            {
                case 1:
                    return entry.Day1;
                case 2:
                    return entry.Day2;
                case 3:
                    return entry.Day3;
                case 4:
                    return entry.Day4;
                case 5:
                    return entry.Day5;
            }

            return null;
        }
    }
}