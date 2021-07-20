using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction
{
    public class DeleteScenarioActionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}