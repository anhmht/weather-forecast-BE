using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWindDirectionService
    {   
        Task<WindDirectionPredictionResponse> GetWindDirectionByDiemId(string diemDuBaoId);
    }
}