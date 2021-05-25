using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class StatusDto
    {
        public Guid StatusId { get; set; }
        public string Name { get; set; }
    }
}