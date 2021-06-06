using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWeatherService
    {
        Task<List<DiemDuBaoResponse>> GetDiemDuBaosList();
        Task<NhietDoResponse> GetNhietDoBy(string diemDuBaoId);      

        Task<WeatherPredictionResponse> GetWeatherByDiemId(string diemDuBaoId);
    }
}