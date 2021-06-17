using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Persistence.Repositories;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Variables
        private readonly GloboWeatherDbContext _context;
        private bool _disposed;
        #endregion

        #region Constructor
        public UnitOfWork(GloboWeatherDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public GloboWeatherDbContext GetContext()
        {
            return _context;
        }

        public async Task<int> CommitAsync([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            int affectedCount = await _context.SaveChangesAsync();

            //if (affectedCount > 0)
            //{
            //    var callerTypeName = Path.GetFileNameWithoutExtension(callerFilePath);
            //    var logMessage = $"{callerTypeName}.{memberName}";
            //}

            return affectedCount;
        }

        #endregion

        #region Implement IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Repositories

        private IBackgroundServiceTrackingRepository _BackgroundServiceTrackingRepository;
        IBackgroundServiceTrackingRepository IUnitOfWork.BackgroundServiceTrackingRepository => _BackgroundServiceTrackingRepository ?? new BackgroundServiceTrackingRepository(_context, this);

        private ICategoryRepository _categoryRepository;
        ICategoryRepository IUnitOfWork.CategoryRepository => _categoryRepository ?? new CategoryRepository(_context, this);

        private IConfigurationRepository _configurationRepository;
        IConfigurationRepository IUnitOfWork.ConfigurationRepository => _configurationRepository ?? new ConfigurationRepository(_context, this);

        private IEventRepository _eventRepository;
        IEventRepository IUnitOfWork.EventRepository => _eventRepository ?? new EventRepository(_context, this);

        private IForumRepository _forumRepository;
        IForumRepository IUnitOfWork.ForumRepository => _forumRepository ?? new ForumRepository(_context, this);

        private IForumCategoryRepository _forumCategoryRepository;
        IForumCategoryRepository IUnitOfWork.ForumCategoryRepository => _forumCategoryRepository ?? new ForumCategoryRepository(_context, this);

        private IForumPostRepository _forumPostRepository;
        IForumPostRepository IUnitOfWork.ForumPostRepository => _forumPostRepository ?? new ForumPostRepository(_context, this);

        private IForumTopicRepository _forumTopicRepository;
        IForumTopicRepository IUnitOfWork.ForumTopicRepository => _forumTopicRepository ?? new ForumTopicRepository(_context, this);

        private IScenarioRepository _scenarioRepository;
        IScenarioRepository IUnitOfWork.ScenarioRepository => _scenarioRepository ?? new ScenarioRepository(_context, this);

        private ISitePageRepository _sitePageRepository;
        ISitePageRepository IUnitOfWork.SitePageRepository => _sitePageRepository ?? new SitePageRepository(_context, this);

        private IStationRepository _stationRepository;
        IStationRepository IUnitOfWork.StationRepository => _stationRepository ?? new StationRepository(_context, this);

        private IStatusRepository _statusRepository;
        IStatusRepository IUnitOfWork.StatusRepository => _statusRepository ?? new StatusRepository(_context, this);

        private IThemeRepository _themeRepository;
        IThemeRepository IUnitOfWork.ThemeRepository => _themeRepository ?? new ThemeRepository(_context, this);

        private ITopicSubscriptionRepository _topicSubscriptionRepository;
        ITopicSubscriptionRepository IUnitOfWork.TopicSubscriptionRepository => _topicSubscriptionRepository ?? new TopicSubscriptionRepository(_context, this);

        private IUpDownVoteRepository _upDownVoteRepository;
        IUpDownVoteRepository IUnitOfWork.UpDownVoteRepository => _upDownVoteRepository ?? new UpDownVoteRepository(_context, this);

        private IWeatherInformationRepository _weatherInformationRepository;
        IWeatherInformationRepository IUnitOfWork.WeatherInformationRepository => _weatherInformationRepository ?? new WeatherInformationRepository(_context, this);


        //Add more repository here
        #endregion

    }
}
