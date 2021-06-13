using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWindDirectionService
    {   
        Task<WindDirectionPredictionResponse> GetWindDirectionByDiemId(string diemDuBaoId);

        Task<List<WindDirectionResponse>> ListAllAsync();
    }
}