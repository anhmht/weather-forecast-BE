using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IHumidityService
    {   
        Task<HumidityPredictionResponse> GetHumidityMinMaxByDiemId(string diemDuBaoId);

        Task<HumidityResponse> GetHumidityBy(string diemDuBaoId);

        Task<List<HumidityResponse>> ListAllAsync();
    }
}