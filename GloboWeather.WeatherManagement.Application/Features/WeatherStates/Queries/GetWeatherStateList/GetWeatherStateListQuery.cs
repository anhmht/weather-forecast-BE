using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList
{
    public class GetWeatherStateListQuery : IRequest<GetWeatherStateListResponse>
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
        
    }
}