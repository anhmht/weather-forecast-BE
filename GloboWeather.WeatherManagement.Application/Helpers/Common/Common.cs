using System.Collections;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Helpers.Common
{
        public record LinkedResource(string Href);
        public enum LinkedResourceType
        {
            None, 
            Prev,
            Next
        }
    
    public interface ILinkedResource    {
        IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
    }
}