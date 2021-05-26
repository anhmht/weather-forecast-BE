using System;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public record EventListCateStatusVm
    {
        public Guid EventId { get; init; }
        public string Title { get; init; }
        public string ImageUrl { get; init; }
        public DateTime DatePosted { get; init; }
    }
}