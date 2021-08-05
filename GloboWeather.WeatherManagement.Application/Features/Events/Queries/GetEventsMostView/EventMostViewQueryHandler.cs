using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView
{
    public class EventMostViewQueryHandler : IRequestHandler<EventMostViewQuery, EventMostViewResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAuthenticationService _authenticationService;
        public EventMostViewQueryHandler(IEventRepository eventRepository, IAuthenticationService authenticationService)
        {
            _eventRepository = eventRepository;
            _authenticationService = authenticationService;
        }
        public async Task<EventMostViewResponse> Handle(EventMostViewQuery request, CancellationToken cancellationToken)
        {
            var eventsListToReturn = await _eventRepository.GetMostViewAsync(request, cancellationToken);

            var users = await _authenticationService.GetAllUserAsync(true);
            eventsListToReturn.Events.ForEach(entry =>
                {
                    entry.CreatedFullName = users.FirstOrDefault(x => x.UserName == entry.CreatedBy)?.FullName;
                });

            return eventsListToReturn;
        }
    }
}