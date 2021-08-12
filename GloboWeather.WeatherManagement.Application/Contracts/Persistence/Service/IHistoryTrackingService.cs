using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IHistoryTrackingService
    {
        Task SaveAsync(HistoryTracking entry);
        Task SaveAsync(string objectName, object originalData, object updatedData, string description, string ipAddress);
    }
}
