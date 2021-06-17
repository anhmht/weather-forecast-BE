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

        private ICategoryRepository _CategoryRepository;
        ICategoryRepository IUnitOfWork.CategoryRepository => _CategoryRepository ?? new CategoryRepository(_context, this);

        private IConfigurationRepository _ConfigurationRepository;
        IConfigurationRepository IUnitOfWork.ConfigurationRepository => _ConfigurationRepository ?? new ConfigurationRepository(_context, this);

        private IEventRepository _EventRepository;
        IEventRepository IUnitOfWork.EventRepository => _EventRepository ?? new EventRepository(_context, this);

        private IForumRepository _ForumRepository;
        IForumRepository IUnitOfWork.ForumRepository => _ForumRepository ?? new ForumRepository(_context, this);

        private IForumCategoryRepository _ForumCategoryRepository;
        IForumCategoryRepository IUnitOfWork.ForumCategoryRepository => _ForumCategoryRepository ?? new ForumCategoryRepository(_context, this);

        private IForumPostRepository _ForumPostRepository;
        IForumPostRepository IUnitOfWork.ForumPostRepository => _ForumPostRepository ?? new ForumPostRepository(_context, this);

        private IForumTopicRepository _ForumTopicRepository;
        IForumTopicRepository IUnitOfWork.ForumTopicRepository => _ForumTopicRepository ?? new ForumTopicRepository(_context, this);

        private IScenarioRepository _ScenarioRepository;
        IScenarioRepository IUnitOfWork.ScenarioRepository => _ScenarioRepository ?? new ScenarioRepository(_context, this);

        private ISitePageRepository _SitePageRepository;
        ISitePageRepository IUnitOfWork.SitePageRepository => _SitePageRepository ?? new SitePageRepository(_context, this);

        private IStationRepository _StationRepository;
        IStationRepository IUnitOfWork.StationRepository => _StationRepository ?? new StationRepository(_context, this);

        private IStatusRepository _StatusRepository;
        IStatusRepository IUnitOfWork.StatusRepository => _StatusRepository ?? new StatusRepository(_context, this);

        private IThemeRepository _ThemeRepository;
        IThemeRepository IUnitOfWork.ThemeRepository => _ThemeRepository ?? new ThemeRepository(_context, this);

        private ITopicSubscriptionRepository _TopicSubscriptionRepository;
        ITopicSubscriptionRepository IUnitOfWork.TopicSubscriptionRepository => _TopicSubscriptionRepository ?? new TopicSubscriptionRepository(_context, this);

        private IUpDownVoteRepository _UpDownVoteRepository;
        IUpDownVoteRepository IUnitOfWork.UpDownVoteRepository => _UpDownVoteRepository ?? new UpDownVoteRepository(_context, this);

        private IWeatherInformationRepository _weatherInformationRepository;
        IWeatherInformationRepository IUnitOfWork.WeatherInformationRepository => _weatherInformationRepository ?? new WeatherInformationRepository(_context, this);


        //Add more repository here
        #endregion

    }
}
