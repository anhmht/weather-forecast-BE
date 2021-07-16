using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonDetail
{
    public class GetExtremePhenomenonDetailQueryHandler : IRequestHandler<GetExtremePhenomenonDetailQuery, ExtremePhenomenonDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetExtremePhenomenonDetailQueryHandler(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ExtremePhenomenonDetailVm> Handle(GetExtremePhenomenonDetailQuery request, CancellationToken cancellationToken)
        {
            var extremePhenomenon = await _unitOfWork.ExtremePhenomenonRepository.GetByIdAsync(request.Id);

            if (extremePhenomenon == null)
            {
                throw new NotFoundException(nameof(extremePhenomenon), request.Id);
            }

            var detailDto = _mapper.Map<ExtremePhenomenonDetailVm>(extremePhenomenon);

            detailDto.Details =
                (await _unitOfWork.ExtremePhenomenonDetailRepository.GetWhereAsync(x =>
                    x.ExtremePhenomenonId == detailDto.Id, cancellationToken)).ToList();

            return detailDto;
        }
    }
}