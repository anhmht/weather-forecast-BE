using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy
{
    public class GetEventsListByQueryHandler : IRequestHandler<GetEventsListByQuery, GetEventListByResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public GetEventsListByQueryHandler(IMapper mapper, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }
        public async Task<GetEventListByResponse> Handle(GetEventsListByQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetEventsListQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            if (request.Limit == 0)
                request.Limit = int.MaxValue;
            if (request.Page == 0)
                request.Page = 1;

            var eventsList = await _eventRepository.GetByPageAsync(request, cancellationToken);

            return new GetEventListByResponse()
            {
                Events = _mapper.Map<List<EventListCateStatusVm>>(eventsList.Events),
                CurrentPage = eventsList.CurrentPage,
                TotalItems = eventsList.TotalItems,
                TotalPages = eventsList.TotalPages
            };
        }
    }
}