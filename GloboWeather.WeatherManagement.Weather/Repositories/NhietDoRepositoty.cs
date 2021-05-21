using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class NhietDoRepositoty : BaseRepository<Nhietdo>, INhietDoRepository
    {
        public NhietDoRepositoty(thoitietContext dbContext) : base(dbContext)
        {
        }
    }
}