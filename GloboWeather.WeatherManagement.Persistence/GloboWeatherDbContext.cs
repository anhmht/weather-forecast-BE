using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence
{
    public class GloboWeatherDbContext : DbContext
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public GloboWeatherDbContext(DbContextOptions<GloboWeatherDbContext> options) : base(options)
        {

        }

        public GloboWeatherDbContext(DbContextOptions<GloboWeatherDbContext> options, ILoggedInUserService loggedInUserService) : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        // public DbSet<Configuration> Configurations { get; set; }
        // public DbSet<Forum> Forums { get; set; }
        // public DbSet<ForumCategory> ForumCategories { get; set; }
        // public DbSet<ForumPost> ForumPosts { get; set; }
        // public DbSet<ForumTopic> ForumTopics { get; set; }
        // public DbSet<SitePage> Pages { get; set; }
        // public DbSet<Theme> Themes { get; set; }
        // public DbSet<TopicSubscription> TopicSubscriptions { get; set; }
        // public DbSet<UpDownVote> DownVotes { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<WeatherMinMaxData> WeatherMinMaxDatas { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<BackgroundServiceTracking> BackgroundServiceTrackings { get; set; }
        public DbSet<WeatherInformation> WeatherInformations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GloboWeatherDbContext).Assembly);

            var publishGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var DraftGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var PendingGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var privateGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            modelBuilder.Entity<Status>().HasData(new Status
            {
                StatusId = publishGuid,
                Name = "Publish"
            });
            modelBuilder.Entity<Status>().HasData(new Status
            {
                StatusId = DraftGuid,
                Name = "Draft"
            });
            modelBuilder.Entity<Status>().HasData(new Status
            {
                StatusId = PendingGuid,
                Name = "Pending"
            });
            modelBuilder.Entity<Status>().HasData(new Status
            {
                StatusId = privateGuid,
                Name = "Private"
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateDate = DateTime.Now;
                        entry.Entity.CreateBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}