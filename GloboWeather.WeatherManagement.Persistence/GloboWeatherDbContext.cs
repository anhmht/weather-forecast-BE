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

        public GloboWeatherDbContext(DbContextOptions<GloboWeatherDbContext> options, ILoggedInUserService loggedInUserService ) :base(options)
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
        
        public  DbSet<Event> Events { get; set; }
        public  DbSet<Category> Categories { get; set; }
        public  DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GloboWeatherDbContext).Assembly);
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
                    case  EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}