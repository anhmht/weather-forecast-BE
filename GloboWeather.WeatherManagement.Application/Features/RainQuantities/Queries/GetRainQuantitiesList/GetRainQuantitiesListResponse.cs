using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;

namespace GloboWeather.WeatherManagement.Application.Features.RainQuantities.Queries.GetRainQuantitiesList
{
    public class GetRainQuantitiesListResponse
    {
        public int CurrentPage { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public List<RainListVm> Rains { get; init; }
    
    }
}