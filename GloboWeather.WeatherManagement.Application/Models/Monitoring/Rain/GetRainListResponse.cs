using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class GetRainListResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<RainListVm> Rains { get; init; }
    }
}