
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
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
                    configuration.GetConnectionString("GloboWeatherWeatherManagementConnectionString")), ServiceLifetime.Transient);

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IScenarioRepository, ScenarioRepository>();
            services.AddScoped<IWeatherInformationRepository, WeatherInformationRepository>();
            services.AddScoped<IBackgroundServiceTrackingRepository, BackgroundServiceTrackingRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IForumCategoryRepository, ForumCategoryRepository>();
            services.AddScoped<IForumPostRepository, ForumPostRepository>();
            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IForumTopicRepository, ForumTopicRepository>();
            services.AddScoped<ISitePageRepository, SitePageRepository>();
            services.AddScoped<IStationRepository, StationRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ITopicSubscriptionRepository, TopicSubscriptionRepository>();
            services.AddScoped<IUpDownVoteRepository, UpDownVoteRepository>();
            services.AddScoped<IWindRankRepository, WindRankRepository>();

            return services;
        }
    }
}