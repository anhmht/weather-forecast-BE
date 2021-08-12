using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class ApplicationUserCacheKey : ICacheKey<List<ApplicationUserDto>>
    {
        public string CacheKey => $"ApplicationUser_All";
    }
}
