using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;

namespace GloboWeather.WeatherManegement.Application.Contracts.Astronomy
{
    public interface IAstronomyService
    {
        Task<GetAstronomyResponse> GetAstronomyData(GetAstronomyCommand request, CancellationToken cancellationToken);
    }
}