using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological
{
    public class GetHydrologicalListResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<HydrologicalListVm> Hydrologicals { get; init; }
    }
}