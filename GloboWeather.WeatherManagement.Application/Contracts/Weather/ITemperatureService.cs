using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface ITemperatureService
    {   
        Task<TemperaturePredictionResponse> GetTemperatureMinMaxByDiemId(string diemDuBaoId);
        Task<TemperatureResponse> GetTemperatureBy(string diemDuBaoId);

        Task<List<TemperatureResponse>> ListAllAsync();
    }
}