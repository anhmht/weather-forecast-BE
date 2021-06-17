using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherBackgroundService.Worker;

namespace WeatherBackgroundService
{
    public static class WeatherBackgroundServiceRegistration
    {
        public static void AddWeatherBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ImportDataWeatherWorker>();
        }
    }
}
