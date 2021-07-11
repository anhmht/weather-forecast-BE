using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateDetail
{
    public class GetWeatherStateDetailQueryHandler : IRequestHandler<GetWeatherStateDetailQuery, WeatherStateDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetWeatherStateDetailQueryHandler(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<WeatherStateDetailVm> Handle(GetWeatherStateDetailQuery request, CancellationToken cancellationToken)
        {
            var weatherState = await _unitOfWork.WeatherStateRepository.GetByIdAsync(request.Id);

            if (weatherState == null)
            {
                throw new NotFoundException(nameof(WeatherState), request.Id);
            }

            var eventDetailDto = _mapper.Map<WeatherStateDetailVm>(weatherState);

            return eventDetailDto;
        }
    }
}