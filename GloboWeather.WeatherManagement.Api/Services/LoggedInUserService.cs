using System.Security.Claims;
using GloboWeather.WeatherManegement.Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
        }
        public string UserId { get; }
    }
}