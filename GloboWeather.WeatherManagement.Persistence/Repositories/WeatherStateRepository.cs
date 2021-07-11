using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class WeatherStateRepository : BaseRepository<WeatherState>, IWeatherStateRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public WeatherStateRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<GetWeatherStateListResponse> GetByPageAsync(GetWeatherStateListQuery query, CancellationToken token)
        {
            var entities = _unitOfWork.WeatherStateRepository.GetAllQuery();
            
            var collections = await entities
                .AsNoTracking()
                .OrderByDescending(p => p.LastModifiedDate)
                .PaginateAsync(query.Page, query.Limit, token);
            return new GetWeatherStateListResponse
            {
                CurrentPage = collections.CurrentPage,
                TotalPages = collections.TotalPages,
                TotalItems = collections.TotalItems,
                WeatherStates = collections.Items.Select(e => new WeatherStateListVm
                {
                    CreateBy = e.CreateBy,
                    Name = e.Name,
                    ImageUrl = e.ImageUrl,
                    Content = e.Content,
                    Id = e.Id,
                    CreateDate = e.CreateDate,
                    LastModifiedBy = e.LastModifiedBy,
                    LastModifiedDate = e.LastModifiedDate
                }).ToList()
            };

        }

    }
}