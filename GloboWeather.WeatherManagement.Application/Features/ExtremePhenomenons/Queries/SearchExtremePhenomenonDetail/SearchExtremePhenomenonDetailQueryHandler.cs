using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.SearchExtremePhenomenonDetail
{
    public class SearchExtremePhenomenonDetailQueryHandler : IRequestHandler<SearchExtremePhenomenonDetailQuery, SearchExtremePhenomenonDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SearchExtremePhenomenonDetailQueryHandler(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<SearchExtremePhenomenonDetailVm> Handle(SearchExtremePhenomenonDetailQuery request, CancellationToken cancellationToken)
        {
            var extremePhenomenon = await _unitOfWork.ExtremePhenomenonRepository.FindAsync(x =>
                x.DistrictId == request.DistrictId && x.ProvinceId == request.ProvinceId &&
                x.Date == request.Date);

            if (extremePhenomenon == null)
            {
                throw new NotFoundException(nameof(extremePhenomenon)
                    , $"ProvinceId = {request.ProvinceId}, DistrictId = {request.DistrictId}, Date = {request.Date}");
            }

            var detailDto = _mapper.Map<SearchExtremePhenomenonDetailVm>(extremePhenomenon);

            detailDto.Details =
                (await _unitOfWork.ExtremePhenomenonDetailRepository.GetWhereAsync(x =>
                    x.ExtremePhenomenonId == detailDto.Id, cancellationToken)).ToList();

            return detailDto;
        }
    }
}