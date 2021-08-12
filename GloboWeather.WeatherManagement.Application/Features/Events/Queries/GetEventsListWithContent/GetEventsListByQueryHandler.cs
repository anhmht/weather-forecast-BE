using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public class GetEventsListWithContentQueryHandler : IRequestHandler<GetEventsListWithContentQuery, EventListWithContentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public GetEventsListWithContentQueryHandler(IMapper mapper, IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<EventListWithContentResponse> Handle(GetEventsListWithContentQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetEventsListWithContentQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _eventRepository.GetEventListByAsync(request, cancellationToken);
        }
    }
}