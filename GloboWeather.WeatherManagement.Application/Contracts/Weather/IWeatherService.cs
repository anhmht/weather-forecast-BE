using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWeatherService
    {
        Task<List<DiemDuBaoResponse>> GetDiemDuBaosList();
        Task<WeatherPredictionResponse> GetWeatherMinMaxByDiemId(string diemDuBaoId);
        Task<WeatherResponse> GetWeatherBy(string diemDuBaoId);
    }
}