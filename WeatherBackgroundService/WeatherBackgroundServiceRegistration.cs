using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherBackgroundService.Worker;

namespace WeatherBackgroundService
{
    public static class WeatherBackgroundServiceRegistration
    {
        public static void AddWeatherBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ImportDataWeatherWorker>();
            services.AddHostedService<DeleteCloudTempFileWorker>();
            services.AddHostedService<AutoGenerateCacheWorker>();
        }
    }
}
