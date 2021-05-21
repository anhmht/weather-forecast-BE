
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class DiemDuBaoRepository :  BaseRepository<Diemdubao>,IDiemDuBaoRepository
    {
        public DiemDuBaoRepository(thoitietContext dbContext) : base(dbContext)
        {
        }

       
    }
}