using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;

namespace GloboWeather.WeatherManegement.Application.Contracts.Astronomy
{
    public interface ILocationService
    {
        Task<GetAstronomyResponse> GetAstronomyData(GetLocationCommand request, CancellationToken cancellationToken);

        Task<GetLocationResponse> GetCurrentLocation(GetPositionStackLocationCommand request,
            CancellationToken cancellationToken);
    }
}