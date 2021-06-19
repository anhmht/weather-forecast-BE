using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WeatherInformationService : IWeatherInformationService
    {
        private readonly IWeatherInformationRepository _weatherInformationRepository;

        public WeatherInformationService(IWeatherInformationRepository weatherInformationRepository)
        {
            _weatherInformationRepository = weatherInformationRepository;
        }
        public async Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken)
        {
            return await _weatherInformationRepository.GetWeatherInformationHorizontalAsync(request, cancelToken);
        }

        public async Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken)
        {
            return await _weatherInformationRepository.GetWeatherInformationsAsync(request, cancelToken);
        }
    }
}