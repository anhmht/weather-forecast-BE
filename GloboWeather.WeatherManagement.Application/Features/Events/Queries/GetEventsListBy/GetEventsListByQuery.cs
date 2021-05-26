using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class GetEventsListByQuery : IRequest<List<EventListCateStatusVm>>
    {
        public Guid CategoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}