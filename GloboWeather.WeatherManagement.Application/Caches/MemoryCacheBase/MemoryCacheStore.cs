using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class MemoryCacheStore : ICacheStore
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, TimeSpan> _expirationConfiguration;

        public MemoryCacheStore(
            IMemoryCache memoryCache,
            Dictionary<string, TimeSpan> expirationConfiguration)
        {
            _memoryCache = memoryCache;
            _expirationConfiguration = expirationConfiguration;
        }

        public void Add<TItem>(TItem item, ICacheKey<TItem> key)
        {
            var timespan = _expirationConfiguration["Expiration"];

            _memoryCache.Set(key.CacheKey, item, timespan);
        }

        public TItem Get<TItem>(ICacheKey<TItem> key) where TItem : class
        {
            if (_memoryCache.TryGetValue(key.CacheKey, out TItem value))
            {
                return value;
            }

            return null;
        }
    }
}
