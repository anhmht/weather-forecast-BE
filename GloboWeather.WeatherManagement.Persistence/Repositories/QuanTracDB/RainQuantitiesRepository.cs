// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
// using GloboWeather.WeatherManagement.Application.Contracts.Persistence.QuanTracDB;
// using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
// using GloboWeather.WeatherManagement.Application.Features.RainQuantities.Queries.GetRainQuantitiesList;
// using GloboWeather.WeatherManagement.Application.Helpers.Paging;
// using GloboWeather.WeatherManagement.Application.Models.Monitoring;
// using GloboWeather.WeatherManagement.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
//
// namespace GloboWeather.WeatherManagement.Persistence.Repositories.QuanTracDB
// {
//     public class RainQuantitiesRepository : BaseRepository<RainQuantity>, IRainQuantitiesRepository
//     {
//         private readonly IUnitOfWork _unitOfWork;
//
//         public RainQuantitiesRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
//         {
//             _unitOfWork = unitOfWork;
//         }
//
//         public async Task<GetRainQuantitiesListResponse> GetByPageAsync(GetRainQuantitiesListQuery query, CancellationToken token)
//         {
//             var entryDatas = await _dbContext.RainQuantities
//                 .AsNoTracking()
//                 .Where(r => r.StationId.Equals(query.StationId)
//                             && (r.RefDate >= query.DateFrom.Date && r.RefDate <= query.DateTo.Date))
//                 .PaginateAsync(query.Page, query.Limit, new CancellationToken());
//
//             return new GetRainQuantitiesListResponse()
//             {
//                 CurrentPage = entryDatas.CurrentPage,
//                 TotalPages = entryDatas.TotalPages,
//                 TotalItems = entryDatas.TotalPages,
//                 Rains = entryDatas.Items.Select(e => new RainListVm()
//                 {
//                     Date = e.RefDate,
//                     RainQuantity = e.Value
//                 }).ToList()
//             };
//           
//         }
//     }
// }