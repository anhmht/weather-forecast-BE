using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Contracts.Monitoring
{
    public interface IMonitoringService
    {
        Task<List<TramKttvResponse>> GetTramKttvList();
        Task<List<GetRainMinMaxResponse>> GetRainMinMax();
    }
}
