using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWindLevelService
    {   
        Task<WindLevelPredictionResponse> GetWindLevelMinMaxByDiemId(string diemDuBaoId);

        Task<WindLevelResponse> GetWindLevelBy(string diemdubaoId);

        Task<List<WinLevelResponse>> ListAllAsync();

    }
}