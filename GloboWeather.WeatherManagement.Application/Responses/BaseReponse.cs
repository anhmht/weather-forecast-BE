using System.Collections.Generic;

namespace GloboWeather.WeatherManegement.Application.Responses
{
    public class BaseReponse
    {
        public BaseReponse()
        {
            
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}