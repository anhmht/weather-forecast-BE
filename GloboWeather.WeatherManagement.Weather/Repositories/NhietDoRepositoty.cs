using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.weathercontext;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Weather.Repositories
{
    public class NhietDoRepositoty : BaseRepository<Nhietdo>, INhietDoRepository
    {
        public NhietDoRepositoty(thoitietContext dbContext) : base(dbContext)
        {
        }

        public async Task<Nhietdo> GetByIdAndDateAsync(string id, DateTime refDate)
        {
            return await _dbContext.Nhietdo.FirstOrDefaultAsync(n => n.DiemId == id && n.RefDate == refDate);
        }
    }
}