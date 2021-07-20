using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail
{
    public class ScenarioActionDetailVm : ScenarioAction
    {
        public string Action { get; set; }
        public string Method { get; set; }
        public string AreaTypeName { get; set; }
        public List<ScenarioActionDetailDto> ScenarioActionDetails { get; set; }

        public ScenarioActionDetailVm()
        {
            ScenarioActionDetails = new List<ScenarioActionDetailDto>();
        }
    }
}