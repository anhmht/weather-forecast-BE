using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario
{
    public class CreateScenarioCommand : IRequest<Guid>
    {
        public string ScenarioName { get; set; }
        public string ScenarioContent { get; set; }
    }
}