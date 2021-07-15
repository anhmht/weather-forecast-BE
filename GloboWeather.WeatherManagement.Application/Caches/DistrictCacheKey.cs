using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class DistrictCacheKey : ICacheKey<List<District>>
    {
        public string CacheKey => $"District_All";
    }
}
