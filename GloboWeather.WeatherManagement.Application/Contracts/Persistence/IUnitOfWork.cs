using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        //GloboWeatherDbContext GetContext();
        Task<int> CommitAsync([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");
        void Dispose();
        IBackgroundServiceTrackingRepository BackgroundServiceTrackingRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IConfigurationRepository ConfigurationRepository { get; }
        IEventRepository EventRepository { get; }
        IForumRepository ForumRepository { get; }
        IForumCategoryRepository ForumCategoryRepository { get; }
        IForumPostRepository ForumPostRepository { get; }
        IForumTopicRepository ForumTopicRepository { get; }
        IScenarioRepository ScenarioRepository { get; }
        ISitePageRepository SitePageRepository { get; }
        IStationRepository StationRepository { get; }
        IStatusRepository StatusRepository { get; }
        IThemeRepository ThemeRepository { get; }
        ITopicSubscriptionRepository TopicSubscriptionRepository { get; }
        IUpDownVoteRepository UpDownVoteRepository { get; }
        IWeatherInformationRepository WeatherInformationRepository { get; }
        IWindRankRepository WindRankRepository { get; }
        IHydrologicalForeCastRepository HydrologicalForeCastRepository { get; }
        IHydrologicalRepository HydrologicalRepository { get; }
        IRainQuantityRepository RainQuantityRepository { get; }
        IMeteorologicalRepository MeteorologicalRepository { get; }
        IWeatherStateRepository WeatherStateRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IExtremePhenomenonRepository ExtremePhenomenonRepository { get; }
        IExtremePhenomenonDetailRepository ExtremePhenomenonDetailRepository { get; }
        IEventDocumentRepository EventDocumentRepository { get; }
        IScenarioActionRepository ScenarioActionRepository { get; }
        IScenarioActionDetailRepository ScenarioActionDetailRepository { get; }
        ICommonLookupRepository CommonLookupRepository { get; }
        IEventViewCountRepository EventViewCountRepository { get; }
        IPostRepository PostRepository { get; }
    }
}