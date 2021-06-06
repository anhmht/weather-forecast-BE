using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.IRepository
{
    public interface ITemperatureRepository : IAsyncRepository<Nhietdo>
    {
        Task<Nhietdo> GetByIdAndDateAsync(string id, DateTime refDate);
       
    }
}