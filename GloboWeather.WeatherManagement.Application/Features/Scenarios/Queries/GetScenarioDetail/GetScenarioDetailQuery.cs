using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail
{
    public class GetScenarioDetailQuery : IRequest<ScenarioDetailVm>
    {
        public Guid ScenarioId { get; set; }
    }
}