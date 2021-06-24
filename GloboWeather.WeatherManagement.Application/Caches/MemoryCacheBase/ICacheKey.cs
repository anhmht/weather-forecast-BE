namespace GloboWeather.WeatherManagement.Application.Caches
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
    }
}
