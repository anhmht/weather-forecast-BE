using System;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail
{
    public class ScenarioDetailVm
    {
        public Guid ScenarioId { get; set; }
        public string ScenarioContent { get; set; }
        public string ScenarioName { get; set; }
    }
}