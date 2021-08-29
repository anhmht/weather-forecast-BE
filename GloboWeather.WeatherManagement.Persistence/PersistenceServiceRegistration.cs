using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Persistence.Repositories;
using GloboWeather.WeatherManagement.Persistence.Repositories.Social;
using GloboWeather.WeatherManagement.Persistence.Services;
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
            services.AddScoped<IHydrologicalForeCastRepository, HydrologicalForeCastRepository>();
            services.AddScoped<IHydrologicalRepository, HydrologicalRepository>();
            services.AddScoped<IRainQuantityRepository, RainQuantityRepository>();
            services.AddScoped<IMeteorologicalRepository, MeteorologicalRepository>();
            services.AddScoped<IWeatherStateRepository, WeatherStateRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IExtremePhenomenonRepository, ExtremePhenomenonRepository>();
            services.AddScoped<IExtremePhenomenonDetailRepository, ExtremePhenomenonDetailRepository>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IEventDocumentRepository, EventDocumentRepository>();
            services.AddScoped<IScenarioActionRepository, ScenarioActionRepository>();
            services.AddScoped<IScenarioActionDetailRepository, ScenarioActionDetailRepository>();
            services.AddScoped<ICommonLookupRepository, CommonLookupRepository>();
            services.AddScoped<IScenarioService, ScenarioService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPostActionIconRepository, PostActionIconRepository>();
            services.AddScoped<IDeleteFileRepository, DeleteFileRepository>();
            services.AddScoped<IHistoryTrackingRepository, HistoryTrackingRepository>();
            services.AddScoped<IHistoryTrackingService, HistoryTrackingService>();
            services.AddScoped<ISocialNotificationRepository, SocialNotificationRepository>();

            return services;
        }
    }
}