using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.IRepository;
using GloboWeather.WeatherManagement.Monitoring.Repository;
using GloboWeather.WeatherManagement.Monitoring.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Monitoring
{
    public static class MonitoringServiceRegistration
    {
        public static void AddMonitoringService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MonitoringContext>(options => options.UseMySQL(configuration.GetConnectionString("GloboQuanTracManagementConnectionString"),
                b => b.MigrationsAssembly(typeof(MonitoringContext).Assembly.FullName)));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IMonitoringService, MonitoringService>();
            services.AddTransient(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITramKttvRepository, TramKttvRepository>();
            services.AddScoped<IRainRepository, RainRepository>();

        }
    }
}
