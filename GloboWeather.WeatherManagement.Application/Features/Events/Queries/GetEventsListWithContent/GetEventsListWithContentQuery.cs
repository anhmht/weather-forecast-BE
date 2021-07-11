using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent
{
    public class GetEventsListWithContentQuery : IRequest<List<EventListWithContentVm>>
    {
        public Guid CategoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}