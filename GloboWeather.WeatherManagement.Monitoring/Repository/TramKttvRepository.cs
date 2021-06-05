using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Monitoring.Repository
{
    public class TramKttvRepository : BaseRepository<TramKttv>, ITramKttvRepository
    {
        public TramKttvRepository(MonitoringContext dbContext) : base(dbContext)
        {
        }


    }
}
