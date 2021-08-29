using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Application.SignalRClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace SignalRClient
{
    public class SignalRClient : ISignalRClient
    {
        private readonly HubConnection _connection;

        public SignalRClient(IOptions<SignalRSetting> signalRSetting)
        {
            var serviceUrl = signalRSetting.Value.ServiceUrl;

#if DEBUG
            serviceUrl = @"https://localhost:44327/notifications";
#endif
            _connection = new HubConnectionBuilder()
                .WithUrl(serviceUrl)
                .Build();
        }

        public async Task StartConnectAsync(string userName)
        {
            await _connection.StartAsync();
            await _connection.InvokeAsync("JoinGroup",
                userName);
        }

        public async Task SendMessageToUser(string userName, string message)
        {
            await _connection.InvokeAsync("SendMessage",
                userName, message);
        }
        public async Task SendMessageToUser2(string userName, string message)
        {
            await _connection.InvokeAsync("SendMessage",
                userName, message);
        }
    }
}
