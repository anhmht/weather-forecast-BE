using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.SignalR
{
    public interface INotificationClient
    {        
        Task ReceiveMessage(object message);      
    }
}
