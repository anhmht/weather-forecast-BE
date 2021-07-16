using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList
{
    public class GetExtremePhenomenonListQueryHandler:IRequestHandler<GetExtremePhenomenonListQuery, GetExtremePhenomenonListResponse>
    {
        private readonly IExtremePhenomenonRepository _extremePhenomenonRepository;
        public GetExtremePhenomenonListQueryHandler(IExtremePhenomenonRepository extremePhenomenonRepository)
        {
            _extremePhenomenonRepository = extremePhenomenonRepository;
        }
        public async Task<GetExtremePhenomenonListResponse> Handle(GetExtremePhenomenonListQuery request, CancellationToken cancellationToken)
        {
            var  extremePhenomenonsListToReturn = await  _extremePhenomenonRepository.GetByPageAsync(request, cancellationToken);
            
            return extremePhenomenonsListToReturn;
        }
    }
}