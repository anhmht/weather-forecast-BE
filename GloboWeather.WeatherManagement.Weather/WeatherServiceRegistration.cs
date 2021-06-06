using System.Reflection;
using AutoMapper;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManagement.Weather.Repositories;
using GloboWeather.WeatherManagement.Weather.Services;
using GloboWeather.WeatherManagement.Weather.weathercontext;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Weather
{
    public static class WeatherServiceRegistration
    {
        public static void AddWeatherService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<thoitietContext>(options => options.UseMySQL(configuration.GetConnectionString("GloboWeatherManagementConnectionString"),
                b => b.MigrationsAssembly(typeof(thoitietContext).Assembly.FullName)));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // weather
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<IHumidityService, HumidityService>();
            services.AddTransient<IRainAmountService, RainAmountService>();
            services.AddTransient<ITemperatureService, TemperatureService>();
            services.AddTransient<IWindLevelService, WindLevelService>();
            services.AddTransient<IWindSpeedService, WindSpeedService>();

            services.AddTransient(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IDiemDuBaoRepository, DiemDuBaoRepository>();


            services.AddScoped<INhietDoRepository, NhietDoRepository>();
            services.AddScoped<IDoAmTBRepository, DoAmTBRepository>();
            services.AddScoped<IGioGiatRepository, GioGiatRepository>();
            services.AddScoped<IRainAmountRepository, RainAmountRepository>();
            services.AddScoped<IThoiTietRepository, ThoiTietRepository>();
            services.AddScoped<ITocDoGioRepository, TocDoGioRepository>();

        }
    }
}