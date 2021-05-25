using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries
{
    public class GetStatusesListQueryHandler :IRequestHandler<GetStatusesListQuery, List<StatusesListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Status> _statusRepository;

        public GetStatusesListQueryHandler(IMapper mapper, IAsyncRepository<Status> statusRepository)
        {
            _mapper = mapper;
            _statusRepository = statusRepository;
        }
        public async Task<List<StatusesListVm>> Handle(GetStatusesListQuery request, CancellationToken cancellationToken)
        {
            var allStatuses = await _statusRepository.ListAllAsync();
            return _mapper.Map<List<StatusesListVm>>(allStatuses);
        }
    }
}