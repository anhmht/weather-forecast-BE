using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IWindRankRepository : IAsyncRepository<WindRank>
    {
        Task<int> DownloadDataAsync(List<WindRank> windRanks);
    }
}