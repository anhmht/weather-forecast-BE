using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQueryHandler:IRequestHandler<GetEventsListQuery, GetEventsListResponse>
    {
        private readonly IEventRepository _eventRepository;
        public GetEventsListQueryHandler(IMapper mapper, IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<GetEventsListResponse> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
        {
            var  eventsListToReturn = await  _eventRepository.GetByPageAsync(request.Limit, request.Page, cancellationToken);
            
            return eventsListToReturn;
        }
    }
}