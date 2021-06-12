
using GloboWeather.WeatherManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IWeatherInformationRepository : IAsyncRepository<WeatherInformation>
    {
        Task<IEnumerable<WeatherInformation>> GetByRefDateStationAsync(DateTime startDate, DateTime endDate, List<string> stationIds, CancellationToken token);
    }
}