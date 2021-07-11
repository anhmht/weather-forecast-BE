using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using LinkedResource = GloboWeather.WeatherManagement.Application.Helpers.Common.LinkedResource;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList
{
    public record GetWeatherStateListResponse : ILinkedResource
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<WeatherStateListVm> WeatherStates { get; init; }
        public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
    }
}