using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList
{
    public record GetExtremePhenomenonListResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<ExtremePhenomenonListVm> ExtremePhenomenons { get; init; }
    }
}