using System;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList
{
    public class ScenariosListVm
    {
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public string ScenarioContent { get; set; }
    }
}