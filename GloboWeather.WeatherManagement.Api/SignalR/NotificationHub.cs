using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.SignalR
{
    public class NotificationHub : Hub<INotificationClient>
    {
 
        public NotificationHub()
        {
            
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage("đsd", "adadadad");
        }



        public override async Task OnConnectedAsync()
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
                await base.OnConnectedAsync();
               
            }
            catch (Exception ex)
            {
                               
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

                
            }

        }
    }
}
