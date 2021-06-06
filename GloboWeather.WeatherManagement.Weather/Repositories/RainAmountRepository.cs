using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class RainAmountRepository : BaseRepository<RainAmount>, IRainAmountRepository
    {
        public RainAmountRepository(thoitietContext dbContext) : base(dbContext)
        {
        }

    }
}
