using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario
{
    public class UpdateScenarioCommand : IRequest
    {
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public string ScenarioContent { get; set; }
    }
}