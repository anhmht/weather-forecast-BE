using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IWeatherInformationRepository : IAsyncRepository<WeatherInformation>
    {
        Task<IEnumerable<WeatherInformation>> GetByRefDateStationAsync(DateTime startDate, DateTime endDate, IEnumerable<string> stationIds, CancellationToken token);

        Task SyncWinLevelAsync(List<WinLevelResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncHumidityAsync(List<HumidityResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncWindDirectionAsync(List<WindDirectionResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncWindSpeedAsync(List<WindSpeedResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncTemperatureAsync(List<TemperatureResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncRainAmountAsync(List<RainAmountResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);

        Task SyncWeatherAsync(List<WeatherResponse> WeatherInformations, DateTime lastUpdate, bool isSaveDb = false);
        Task ImportAsync(List<WeatherInformation> importData, CancellationToken token);
        Task<GetWeatherInformationResponse> ImportSingleStationAsync(string stationId, string stationName, List<WeatherInformation> importData, CancellationToken token);
        Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken);
        Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken);
    }
}