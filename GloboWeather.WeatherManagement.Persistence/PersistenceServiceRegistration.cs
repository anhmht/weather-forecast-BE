using GloboWeather.WeatherManagement.Persistence.Repositories;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<GloboWeatherDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("GloboWeatherWeatherManagementConnectionString")));
            
            // services.AddDbContext<ThoiTietDbContext>(options =>
            //     options.UseMySQL(
            //         configuration.GetConnectionString("GloboWeatherWeatherManagementConnectionString")));
            
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            //services.AddScoped(typeof(IAsyncRepositoryVN<>), typeof(BaseRepositoryVN<>));
            services.AddScoped<IEventRepository, EventRepository>();
           // services.AddScoped<IDiemDuBaoRepository, DiemDuBaoRepository>();
            
            return services;
        }
    }
}