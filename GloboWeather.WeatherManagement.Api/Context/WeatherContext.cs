using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.Context
{
    public class WeatherContext
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

       public string SignalRSessionId { get; set; }

        public void Release()
        {
        }
    }
}
