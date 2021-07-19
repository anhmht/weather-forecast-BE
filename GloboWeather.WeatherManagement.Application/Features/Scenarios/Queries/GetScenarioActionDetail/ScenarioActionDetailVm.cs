using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail
{
    public class ScenarioActionDetailVm
    {
        public Guid ScenarioId { get; set; }
        public string ScenarioContent { get; set; }
        public string ScenarioName { get; set; }
        public List<ScenarioActionDto> ScenarioActions { get; set; }

        public ScenarioActionDetailVm()
        {
            ScenarioActions = new List<ScenarioActionDto>();
        }
    }

    public class ScenarioActionDto : ScenarioAction
    {
        public string Action { get; set; }
        public string Method { get; set; }
        public string AreaTypeName { get; set; }
        public List<ScenarioActionDetailDto> ScenarioActionDetails { get; set; }

        public ScenarioActionDto()
        {
            ScenarioActionDetails = new List<ScenarioActionDetailDto>();
        }
    }

    public class ScenarioActionDetailDto : ScenarioActionDetail
    {
        public string ScenarioActionTypeName { get; set; }
        public string Posision { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public List<string> IconsList { get; set; }

        public ScenarioActionDetailDto()
        {
            IconsList = new List<string>();
        }
    }
}