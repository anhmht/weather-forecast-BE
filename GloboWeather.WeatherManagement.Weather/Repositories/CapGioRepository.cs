
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class CapGioRepository :  BaseRepository<Capgio>,ICapGioRepository
    {
        public CapGioRepository(thoitietContext dbContext) : base(dbContext)
        {
        }

       
    }
}