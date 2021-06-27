using System;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;

namespace GloboWeather.WeatherManagement.Persistence.IntegrationTests
{
    public class GloboWeatherDbContextTests
    {
        private readonly GloboWeatherDbContext _globoWeatherDbContext;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _loggedInUserId;

        public GloboWeatherDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<GloboWeatherDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _loggedInUserId = "00000000-0000-0000-0000-000000000000";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);

            _globoWeatherDbContext = new GloboWeatherDbContext(dbContextOptions, _loggedInUserServiceMock.Object);
        }

        [Fact]
        public async void Save_SetCreatedByProperty()
        {
            var ev = new Event() {EventId = Guid.NewGuid(), Title = "Test Event"};
            _globoWeatherDbContext.Events.Add(ev);
            await _globoWeatherDbContext.SaveChangesAsync();
            ev.CreateBy.ShouldBe(_loggedInUserId);
        }
    }
}