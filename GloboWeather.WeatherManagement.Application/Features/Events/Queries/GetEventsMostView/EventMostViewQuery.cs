using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView
{
    public class EventMostViewQuery : IRequest<EventMostViewResponse>
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public int DayNumber { get; set; }

    }
}