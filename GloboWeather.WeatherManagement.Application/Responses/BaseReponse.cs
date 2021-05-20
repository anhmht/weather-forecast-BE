using System.Collections.Generic;

namespace GloboWeather.WeatherManegement.Application.Responses
{
    public class BaseReponse
    {
        public BaseReponse()
        {
            Success = true;
        }

        public BaseReponse(string message = null)
        {
            Success = true;
            Message = message;
        }

        public BaseReponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}