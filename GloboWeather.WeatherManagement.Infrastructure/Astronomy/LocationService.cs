using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManagement.Application.Models.PositionStack;
using GloboWeather.WeatherManegement.Application.Contracts.Astronomy;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.Astronomy
{
    public class LocationService : ILocationService
    {
        public  AstronomySettings _astronomySetting;
        private readonly HttpClient _httpClient;
        public PositionStackSettings _positionSettings;

        public LocationService(IOptions<AstronomySettings> astronomySetting,
            HttpClient httpClient,
            IOptions<PositionStackSettings> positionSettings)
        {
            _astronomySetting = astronomySetting.Value;
            _httpClient = httpClient;
            _positionSettings = positionSettings.Value;
        }
        public async Task<GetAstronomyResponse> GetAstronomyData(GetLocationCommand request, CancellationToken cancellationToken)
        {
            string uri =
                $"https://api.weatherapi.com/v1/astronomy.json?key={_astronomySetting.Key}&q={request.Lat},{request.Lon}&dt={DateTime.Today}";
            return await _httpClient.GetFromJsonAsync<GetAstronomyResponse>(uri, cancellationToken);
        }

        public async Task<GetLocationResponse> GetCurrentLocation(GetPositionStackLocationCommand request, CancellationToken cancellationToken)
        {
            string uri =
                $"http://api.positionstack.com/v1/reverse?access_key={_positionSettings.AccessKey}&query={request.Lat},{request.Lon}";
            return await _httpClient.GetFromJsonAsync<GetLocationResponse>(uri, cancellationToken);
        }
    }
}