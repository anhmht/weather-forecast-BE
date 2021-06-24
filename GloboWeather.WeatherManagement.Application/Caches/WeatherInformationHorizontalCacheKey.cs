using System.Linq;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManagement.Application.Helpers.Common;

namespace GloboWeather.WeatherManagement.Application.Caches
{
    public class WeatherInformationHorizontalCacheKey : ICacheKey<GetWeatherInformationHorizontalResponse>
    {
        private readonly string _key;
        public WeatherInformationHorizontalCacheKey(GetWeatherInformationHorizontalRequest request)
        {
            var stations = request.StationIds?.Count() > 0 ?
                string.Join("_", request.StationIds.OrderBy(x => x)) : string.Empty;
            var weatherTypes = request.WeatherTypes?.Count() > 0 ?
                string.Join("_", request.WeatherTypes.OrderBy(x => x)) : string.Empty;
            _key = Md5Hash.Create($"{request.FromDate}{request.ToDate}{stations}{weatherTypes}");
        }

        public string CacheKey => $"WeatherInformationHorizontal_{_key}";
    }
}
