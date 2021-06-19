using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;

namespace GloboWeather.WeatherManegement.Application.Contracts.Weather
{
    public interface IWeatherInformationService
    {
        Task<GetWeatherInformationResponse> GetWeatherInformationsAsync(GetWeatherInformationRequest request, CancellationToken cancelToken);
        Task<GetWeatherInformationHorizontalResponse> GetWeatherInformationHorizontalAsync(GetWeatherInformationHorizontalRequest request, CancellationToken cancelToken);
    }
}