using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateActionOrder;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IScenarioService
    {
        Task<ScenarioDetailVm> GetScenarioDetailAsync(GetScenarioDetailQuery request);
        Task<ScenarioActionDetailVm> GetScenarioActionDetailAsync(GetScenarioActionDetailQuery request);
        Task<Guid> CreateScenarioActionAsync(CreateScenarioActionCommand request, CancellationToken cancellationToken);
        Task<Guid> UpdateScenarioActionAsync(UpdateScenarioActionCommand request, CancellationToken cancellationToken);
        Task<bool> DeleteScenarioAsync(DeleteScenarioCommand request, CancellationToken cancellationToken);
        Task<bool> DeleteScenarioActionAsync(DeleteScenarioActionCommand request, CancellationToken cancellationToken);
        Task<bool> UpdateActionOrderAsync(UpdateActionOrderCommand request, CancellationToken cancellationToken);
    }
}