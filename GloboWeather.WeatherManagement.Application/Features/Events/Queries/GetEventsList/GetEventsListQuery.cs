using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQuery : IRequest<GetEventsListResponse>
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
        public  Guid? CategoryId { get; set; }
        public  Guid? StatusId { get; set; }
        
    }
}