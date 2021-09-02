using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GloboWeather.WeatherManagement.Application.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string groupName, object message)
        {         
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(string groupName)
        {           
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }


        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client connect error {ex}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client disconnect error {ex}");
            }

        }
    }
}
