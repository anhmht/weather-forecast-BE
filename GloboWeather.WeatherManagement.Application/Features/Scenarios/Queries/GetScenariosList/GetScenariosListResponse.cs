using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList
{
    public class GetScenariosListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<ScenariosListVm> Scenarios { get; init; }
    }
}