using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class GetHumidityRequest : WeatherInformationBaseRequest, IRequest<HumidityResponse>
    {

    }
}