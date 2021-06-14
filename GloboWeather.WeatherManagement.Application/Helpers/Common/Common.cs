using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GloboWeather.WeatherManagement.Application.Helpers.Common
{
    public record LinkedResource(string Href);

    public enum LinkedResourceType
    {
        None,
        Prev,
        Next
    }

    public interface ILinkedResource
    {
        IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
    }

    public static class Forder
    {
        public static string FeatureImage = "feature-Image";
        public static string NormalImage = "normal-Image";
    }

    public static class ReplaceContent
    {
        public static string ReplaceImageUrls(string content, List<string> oldUrls, List<string> newUrls)
        {
            foreach (var newUrl in newUrls)
            {
                var urls = newUrl.Split('/');
                var nameImage = urls[urls.Length - 1];
                var urlsNeedToReplce = oldUrls.FirstOrDefault(x => x.Contains(nameImage));
                if (urlsNeedToReplce != null)
                {
                    content = content.Replace(urlsNeedToReplce, newUrl);
                }
            }

            return content;
        }
    }

    public enum WeatherType
    {
        Humidity = 1,
        WindLevel,
        WindDirection,
        WindSpeed,
        RainAmount,
        Temperature,
        Weather
    }
}