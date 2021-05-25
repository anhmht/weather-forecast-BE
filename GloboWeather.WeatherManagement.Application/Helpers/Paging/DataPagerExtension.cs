using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Application.Helpers.Paging
{
    public static class DataPagerExtension
    {
        public static async Task<PagedModel<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int page,
            int limit,
            CancellationToken cancellationToken) where TModel : class
        {
            var paged = new PagedModel<TModel>();
            page = (page < 0) ? 1 : page;

            paged.CurrentPage = page;
            paged.PageSize = limit;

            var totalItemsCount = await query.CountAsync(cancellationToken);

            var startRow = (page - 1) * limit;
            paged.Items = await query
                .Skip(startRow)
                .Take(limit)
                .ToListAsync(cancellationToken);

            paged.TotalItems =   totalItemsCount;
            paged.TotalPages = (int) Math.Ceiling(paged.TotalItems / (double) limit);

            return paged;
        }
    }
}