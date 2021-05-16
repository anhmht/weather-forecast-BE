using System;

namespace GloboWeather.WeatherManagement.Api
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FileResultContentTypeAttribute: Attribute
    {
        public FileResultContentTypeAttribute(string contentType)
        {
            ContentType = contentType;
        }
        public  string ContentType { get;  }
    }
}