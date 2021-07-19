using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class CommonLookupCacheKey : ICacheKey<List<CommonLookup>>
    {
        public string CacheKey => $"CommonLookup_All";
    }
}
