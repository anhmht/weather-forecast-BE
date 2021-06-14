using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario
{
    public class DeleteScenarioCommand : IRequest
    {
        public Guid ScenarioId { get; set; }
    }
}