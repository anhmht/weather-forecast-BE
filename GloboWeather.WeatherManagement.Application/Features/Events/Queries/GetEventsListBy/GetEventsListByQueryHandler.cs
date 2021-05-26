using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class GetEventsListByQueryHandler : IRequestHandler<GetEventsListByQuery, List<EventListCateStatusVm>>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public GetEventsListByQueryHandler(IMapper mapper, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }
        public async Task<List<EventListCateStatusVm>> Handle(GetEventsListByQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetEventsListQueryValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var eventsList =
                (await _eventRepository.GetEventListByAsync(request.CategoryId, request.StatusId, cancellationToken))
                .OrderByDescending(e => e.DatePosted);

            return _mapper.Map<List<EventListCateStatusVm>>(eventsList);
        }
    }
}