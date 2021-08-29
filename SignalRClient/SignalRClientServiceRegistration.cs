using GloboWeather.WeatherManagement.Application.SignalRClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRClient
{
    public static class SignalRClientServiceRegistration
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ISignalRClient, SignalRClient>();
            return services;
        }
    }
}
