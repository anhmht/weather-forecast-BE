
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IWeatherInformationRepository : IAsyncRepository<WeatherInformation>
    {
    }
}