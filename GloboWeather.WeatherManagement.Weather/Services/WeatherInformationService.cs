using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Caches;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WeatherInformationService : IWeatherInformationService
    {
        private readonly IWeatherInformationRepository _weatherInformationRepository;
        private readonly ICacheStore _cacheStore;

        public WeatherInformationService(IWeatherInformationRepository weatherInformationRepository
            , ICacheStore cacheStore)
        {
            _weatherInformationRepository = weatherInformationRepository;
            _cacheStore = cacheStore;
        }
        public async Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken)
        {
            RequestHelper.StandadizeGetWeatherInformationBaseRequest(request, true);
            // Get from cache
            var weatherInformationHorizontalCacheKey = new WeatherInformationHorizontalCacheKey(request);
            var cachedGetWeatherInformationHorizontalResponse = _cacheStore.Get(weatherInformationHorizontalCacheKey);
            if (cachedGetWeatherInformationHorizontalResponse != null)
                return cachedGetWeatherInformationHorizontalResponse;

            //Get from database
            var response = await _weatherInformationRepository.GetWeatherInformationHorizontalAsync(request, cancelToken);

            //Save cache
            _cacheStore.Add(response, weatherInformationHorizontalCacheKey);

            return response;
        }

        public async Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken)
        {
            RequestHelper.StandadizeGetWeatherInformationBaseRequest(request);
            // Get from cache
            var weatherInformationCacheKey = new WeatherInformationCacheKey(request);
            var cachedGetWeatherInformationResponse = _cacheStore.Get(weatherInformationCacheKey);
            if (cachedGetWeatherInformationResponse != null)
                return cachedGetWeatherInformationResponse;
            
            //Get from database
            var response = await _weatherInformationRepository.GetWeatherInformationsAsync(request, cancelToken);

            //Save cache
            _cacheStore.Add(response, weatherInformationCacheKey);

            return response;
        }

    }
}