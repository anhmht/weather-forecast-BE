using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class ProvinceCacheKey : ICacheKey<List<Province>>
    {
        public string CacheKey => $"Province_All";
    }
}
