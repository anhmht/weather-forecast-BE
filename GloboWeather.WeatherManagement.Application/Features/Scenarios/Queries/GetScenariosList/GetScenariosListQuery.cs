using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList
{
    public class GetScenariosListQuery : IRequest<GetScenariosListResponse>
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
    }
}