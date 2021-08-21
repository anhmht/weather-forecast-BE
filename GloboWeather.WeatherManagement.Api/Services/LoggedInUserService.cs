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
            IpAddress =
                $"{httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress}:{httpContextAccessor.HttpContext?.Connection?.RemotePort}";
        }
        public string UserId { get; }
        public string IpAddress { get; }
    }
}