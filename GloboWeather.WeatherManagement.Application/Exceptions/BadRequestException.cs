using System;

namespace GloboWeather.WeatherManagement.Application.Exceptions
{
    public class BadRequestException: ApplicationException
    {
        public BadRequestException(string message): base(message)
        {
            
        }
    }
}