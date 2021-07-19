using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail
{
    public class GetScenarioActionDetailQuery : IRequest<ScenarioActionDetailVm>
    {
        public Guid ScenarioId { get; set; }
    }
}