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
        public AstronomySettings _astronomySetting;
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

        public async Task<GetAstronomyResponse> GetAstronomyData(GetLocationCommand request,
            CancellationToken cancellationToken)
        {
            string uri =
                $"https://api.weatherapi.com/v1/astronomy.json?key={_astronomySetting.Key}&q={request.Lat},{request.Lon}&dt={DateTime.Today}";
            return await _httpClient.GetFromJsonAsync<GetAstronomyResponse>(uri, cancellationToken);
        }

        public async Task<GetLocationResponse> GetCurrentLocation(GetPositionStackLocationCommand request,
            CancellationToken cancellationToken)
        {
            string uriIpStack =
                $"http://api.ipstack.com/{request.IpAddress}?access_key={_positionSettings.AccessKeyForIp}";
            
            var httpRequestForIp = new HttpRequestMessage(HttpMethod.Get, uriIpStack);
            using var responseForIp = await _httpClient.SendAsync(httpRequestForIp,
                HttpCompletionOption.ResponseContentRead, cancellationToken);
            
            if (!responseForIp.IsSuccessStatusCode) return null;

            var result =
                await responseForIp.Content.ReadFromJsonAsync<GetLocationResponse>(
                    cancellationToken: cancellationToken);

            if (result == null || !string.IsNullOrEmpty(result.RegionCode)) return result;
            result.RegionCode = "SG";
            result.RegionName = "Ho Chi Minh";

            return result;
        }
    }
}