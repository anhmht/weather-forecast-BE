using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList
{
    public class GetWeatherStateListQueryHandler:IRequestHandler<GetWeatherStateListQuery, GetWeatherStateListResponse>
    {
        private readonly IWeatherStateRepository _weatherStateRepository;
        public GetWeatherStateListQueryHandler(IWeatherStateRepository weatherStateRepository)
        {
            _weatherStateRepository = weatherStateRepository;
        }
        public async Task<GetWeatherStateListResponse> Handle(GetWeatherStateListQuery request, CancellationToken cancellationToken)
        {
            var  weatherStatesListToReturn = await  _weatherStateRepository.GetByPageAsync(request, cancellationToken);
            
            return weatherStatesListToReturn;
        }
    }
}