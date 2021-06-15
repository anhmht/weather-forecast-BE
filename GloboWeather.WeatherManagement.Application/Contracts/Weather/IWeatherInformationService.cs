using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWeatherInformationService
    {
        Task<HumidityPredictionResponse> GetHumidityAsync(string stationId, DateTime fromDate, DateTime toDate, CancellationToken cancelToken);
        Task<T> GetWeatherInformationVerticalAsync<T>(string stationId, DateTime fromDate, DateTime toDate, WeatherType weatherType, CancellationToken cancelToken);

        Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken);
        Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken);
    }
}