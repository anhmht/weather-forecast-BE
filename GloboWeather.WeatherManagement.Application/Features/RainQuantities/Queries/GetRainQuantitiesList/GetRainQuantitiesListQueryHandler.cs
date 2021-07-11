using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.QuanTracDB;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.RainQuantities.Queries.GetRainQuantitiesList
{
    // public class
    //     GetRainQuantitiesListQueryHandler : IRequestHandler<GetRainQuantitiesListQuery, GetRainQuantitiesListResponse>
    // {
    //     private readonly IRainQuantitiesRepository _rainQuantitiesRepository;
    //
    //     public GetRainQuantitiesListQueryHandler(IRainQuantitiesRepository rainQuantitiesRepository)
    //     {
    //         _rainQuantitiesRepository = rainQuantitiesRepository;
    //     }
    //
    //     public async Task<GetRainQuantitiesListResponse> Handle(GetRainQuantitiesListQuery request,
    //         CancellationToken cancellationToken)
    //     {
    //         var rainQuantitiesListToReturn = await _rainQuantitiesRepository.GetByPageAsync(request, cancellationToken);
    //
    //         return rainQuantitiesListToReturn;
    //     }
    // }
}