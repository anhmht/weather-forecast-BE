using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IRainAmountService
    {   
        Task<RainAmountPredictionResponse> GetRainAmountMinMaxByDiemId(string diemDuBaoId);
        Task<AmountOfRainResponse> GetAmountOfRainBy(string diemDuBaoId);

        Task<List<RainAmountResponse>> ListAllAsync();
    }
}