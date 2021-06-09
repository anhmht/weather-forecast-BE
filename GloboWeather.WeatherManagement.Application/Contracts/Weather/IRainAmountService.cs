using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IRainAmountService
    {   
        Task<RainAmountPredictionResponse> GetRainAmountMinMaxByDiemId(string diemDuBaoId);
        Task<AmountOfRainResponse> GetAmountOfRainBy(string diemDuBaoId);
    }
}