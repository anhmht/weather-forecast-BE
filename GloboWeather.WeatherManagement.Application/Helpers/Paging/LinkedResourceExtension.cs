using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Helpers.Common;

namespace GloboWeather.WeatherManagement.Application.Helpers.Paging
{
    public static class LinkedResourceExtension
    {
        public static void AddResourceLink(this ILinkedResource resources,
            LinkedResourceType resourceType,
            string routeUrl)
        {
            resources.Links ??= new Dictionary<LinkedResourceType, LinkedResource>();
            resources.Links[resourceType] = new LinkedResource(routeUrl);

        }
    }
}