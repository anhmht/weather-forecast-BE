using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQueryHandler : IRequestHandler<GetEventsListQuery, GetEventsListResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAuthenticationService _authenticationService;
        public GetEventsListQueryHandler(IEventRepository eventRepository, IAuthenticationService authenticationService)
        {
            _eventRepository = eventRepository;
            _authenticationService = authenticationService;
        }
        public async Task<GetEventsListResponse> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
        {
            var eventsListToReturn = await _eventRepository.GetByPageAsync(request, cancellationToken);

            var users = await _authenticationService.GetAllUserAsync(true);
            eventsListToReturn.Events.ForEach(entry =>
                {
                    entry.CreatedFullName = users.FirstOrDefault(x => x.UserName == entry.CreatedBy)?.FullName;
                });

            return eventsListToReturn;
        }
    }
}