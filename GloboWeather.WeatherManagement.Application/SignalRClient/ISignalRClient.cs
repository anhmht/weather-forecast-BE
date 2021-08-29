using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.SignalRClient
{
    public interface ISignalRClient
    {
        Task StartConnectAsync(string userName);
        Task SendMessageToUser(string userName, string message);
    }
}