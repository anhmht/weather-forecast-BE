using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        //GloboWeatherDbContext GetContext();
        Task<int> CommitAsync([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");
        void Dispose();
        //Repository<Category> CategoryRepository { get; }
        //Repository<Configuration> ConfigurationRepository { get; }
        //Repository<Event> EventRepository { get; }
        //Repository<Forum> ForumRepository { get; }
        //Repository<ForumCategory> ForumCategoryRepository { get; }
        //Repository<ForumPost> ForumPostRepository { get; }
        //Repository<ForumTopic> ForumTopicRepository { get; }
        //Repository<Scenario> ScenarioRepository { get; }
        //Repository<SitePage> SitePageRepository { get; }
        //Repository<Station> StationRepository { get; }
        //Repository<Status> StatusRepository { get; }
        //Repository<Theme> ThemeRepository { get; }
        //Repository<TopicSubscription> TopicSubscriptionRepository { get; }
        //Repository<UpDownVote> UpDownVoteRepository { get; }
        IRepository<WeatherInformation> WeatherInformationRepository { get; }
    }
}