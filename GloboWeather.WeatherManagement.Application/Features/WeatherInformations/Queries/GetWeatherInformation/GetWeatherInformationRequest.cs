using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationRequest : GetWeatherInformationBaseRequest, IRequest<GetWeatherInformationResponse>
    {

    }
}
