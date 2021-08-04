using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherBackgroundService.Worker;

namespace WeatherBackgroundService
{
    public static class WeatherBackgroundServiceRegistration
    {
        public static void AddWeatherBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            //On develop environment, no need run these service
            //services.AddHostedService<DeleteCloudTempFileWorker>();

            services.AddHostedService<ImportDataWeatherWorker>();
            services.AddHostedService<DownloadMonitoringDataWorker>();
            services.AddHostedService<AutoGenerateCacheWorker>();
        }
    }
}
