using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.IRepository
{
    public interface IRainAmountRepository : IAsyncRepository<RainAmount>
    {
        
    }
}