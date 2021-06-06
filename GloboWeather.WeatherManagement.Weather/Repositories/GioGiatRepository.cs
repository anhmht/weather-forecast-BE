using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class GioGiatRepository : BaseRepository<GioGiat>, IGioGiatRepository
    {
        public GioGiatRepository(thoitietContext dbContext) : base(dbContext)
        {
        }
     
    }
}