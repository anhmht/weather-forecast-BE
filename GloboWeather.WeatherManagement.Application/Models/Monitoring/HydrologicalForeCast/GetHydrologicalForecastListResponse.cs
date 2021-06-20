using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast
{
    public class GetHydrologicalForecastListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<HydrologicalForecastListVm> GetHydrologicalForecasts { get; set; }

        public GetHydrologicalForecastListResponse()
        {
            GetHydrologicalForecasts = new List<HydrologicalForecastListVm>();
        }
    }
}
