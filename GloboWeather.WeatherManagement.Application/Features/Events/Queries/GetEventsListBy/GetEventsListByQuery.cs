using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy
{
    public class GetEventsListByQuery : EventsListQuery, IRequest<GetEventListByResponse>
    {

    }
}