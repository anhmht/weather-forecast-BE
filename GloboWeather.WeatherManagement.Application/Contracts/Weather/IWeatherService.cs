using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWeatherService
    {
        Task<List<DiemDuBaoResponse>> GetDiemDuBaosList();
        Task<WeatherResponse> GetWeatherBy(string diemDuBaoId);

        Task<List<WeatherResponse>> ListAllAsync();
    }
}