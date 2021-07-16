using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class ExtremePhenomenonRepository : BaseRepository<ExtremePhenomenon>, IExtremePhenomenonRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExtremePhenomenonRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetExtremePhenomenonListResponse> GetByPageAsync(GetExtremePhenomenonListQuery query, CancellationToken token)
        {
            var entities = from ep in _unitOfWork.ExtremePhenomenonRepository.GetAllQuery()
                join pr in _unitOfWork.ProvinceRepository.GetAllQuery() on ep.ProvinceId equals pr.Id
                join dt in _unitOfWork.DistrictRepository.GetAllQuery() on ep.DistrictId equals dt.Id
                select new ExtremePhenomenonListVm()
                {
                    Date = ep.Date,
                    Id = ep.Id,
                    CreateBy = ep.CreateBy,
                    ProvinceId = ep.ProvinceId,
                    LastModifiedDate = ep.LastModifiedDate,
                    DistrictId = ep.DistrictId,
                    CreateDate = ep.CreateDate,
                    LastModifiedBy = ep.LastModifiedBy,
                    DistrictName = dt.Name,
                    ProvinceName = pr.Name
                };

            if (query.Date.HasValue)
                entities = entities.Where(x => x.Date.Date == query.Date.Value.Date);
            if (query.ProvinceId.HasValue)
                entities = entities.Where(x => x.ProvinceId == query.ProvinceId.Value);

            var collections = await entities
                .AsNoTracking()
                .OrderByDescending(p => p.LastModifiedDate)
                .PaginateAsync(query.Page, query.Limit, token);
            return new GetExtremePhenomenonListResponse
            {
                CurrentPage = collections.CurrentPage,
                TotalPages = collections.TotalPages,
                TotalItems = collections.TotalItems,
                ExtremePhenomenons = collections.Items.ToList()
            };

        }
    }
}