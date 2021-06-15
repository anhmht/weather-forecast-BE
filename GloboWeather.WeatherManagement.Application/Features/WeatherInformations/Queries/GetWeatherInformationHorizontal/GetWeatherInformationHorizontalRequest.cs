using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationHorizontalRequest : GetWeatherInformationBaseRequest, IRequest<GetWeatherInformationHorizontalResponse>
    {

    }
}
