using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWindSpeedService
    {   
        Task<WindSpeedPredictionResponse> GetWindSpeedMinMaxByDiemId(string diemDuBaoId);
        Task<WindSpeedResponse> GetWindSpeedBy(string diemdubaoId);
    }
}