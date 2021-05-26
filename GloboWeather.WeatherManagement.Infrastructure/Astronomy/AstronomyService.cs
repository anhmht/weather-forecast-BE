using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManegement.Application.Contracts.Astronomy;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.Astronomy
{
    public class AstronomyService : IAstronomyService
    {
        private readonly AstronomySettings _astronomySetting;
        private readonly HttpClient _httpClient;

        public AstronomyService(IOptions<AstronomySettings> astronomySetting, HttpClient httpClient)
        {
            _astronomySetting = astronomySetting.Value;
            _httpClient = httpClient;
        }
        public async Task<GetAstronomyResponse> GetAstronomyData(GetAstronomyCommand request, CancellationToken cancellationToken)
        {
            string uri =
                $"https://api.weatherapi.com/v1/astronomy.json?key={_astronomySetting.Key}&q={request.Lat},{request.Lon}&dt={DateTime.Today}";
            return await _httpClient.GetFromJsonAsync<GetAstronomyResponse>(uri, cancellationToken);
        }
    }
}