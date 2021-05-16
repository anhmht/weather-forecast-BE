using GloboWeather.WeatherManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Identity
{
    public class GloboWeatherIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public GloboWeatherIdentityDbContext(DbContextOptions<GloboWeatherIdentityDbContext> options): base(options)
        {
           
        }
    }
}