using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence
{
    public interface IExtremePhenomenonDetailRepository : IAsyncRepository<ExtremePhenomenonDetail>
    {
        
    }
}