using GloboWeather.WeatherManagement.Api.Context;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.SignalR
{
    public class NotificationHub : Hub<INotificationClient>
    {

        private readonly WeatherContext _weatherContext;
       
        private readonly IServiceProvider _serviceProvider;
     //   private string groupNameDemo = "Demono";
        public NotificationHub(WeatherContext weatherContext, IServiceProvider serviceProvider)
        {
            _weatherContext = weatherContext;
            _serviceProvider = serviceProvider;
           
        }

        public async Task SendMessage(string groupName, object message)
        {
           // await Clients.All.ReceiveMessage(message);
            await Clients.Group(groupName).ReceiveMessage(message);            
        }

        public async Task JoinGroup(string groupName)
        {
           
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

           // await Clients.Group(groupName).ReceiveMessage("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }


        public override async Task OnConnectedAsync()
        {
            try
            {              
                //await Groups.AddToGroupAsync(Context.ConnectionId, "Gobal");
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
