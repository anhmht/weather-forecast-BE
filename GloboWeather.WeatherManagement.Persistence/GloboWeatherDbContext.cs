using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Common;
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