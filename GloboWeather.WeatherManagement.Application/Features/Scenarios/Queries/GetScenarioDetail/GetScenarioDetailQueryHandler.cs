using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail
{
    public class GetScenarioDetailQueryHandler : IRequestHandler<GetScenarioDetailQuery, ScenarioDetailVm>
    {
        private readonly IScenarioService _scenarioService;
        public GetScenarioDetailQueryHandler(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        public async Task<ScenarioDetailVm> Handle(GetScenarioDetailQuery request, CancellationToken cancellationToken)
        {
            return await _scenarioService.GetScenarioDetailAsync(request);
        }
    }
}