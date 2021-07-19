using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IScenarioService
    {
        Task<ScenarioActionDetailVm> GetDetailAsync(GetScenarioActionDetailQuery request);
        Task<Guid> CreateAsync(CreateScenarioActionCommand request, CancellationToken cancellationToken);
        Task<Guid> UpdateAsync(UpdateScenarioActionCommand request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(DeleteScenarioActionCommand request, CancellationToken cancellationToken);
    }
}