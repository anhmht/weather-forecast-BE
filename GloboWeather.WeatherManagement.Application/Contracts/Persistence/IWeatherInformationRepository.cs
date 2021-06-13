
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IWeatherInformationRepository : IAsyncRepository<WeatherInformation>
    {
        Task UpdateWinLevelAsync(List<WinLevelResponse> WeatherInformations, DateTime lastUpdate);
    }
}