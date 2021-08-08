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
        public static string IconImage = "icon-Image";
        public static string ImportVideo = "import-video";
        public static string Post = "post";
    }

    public static class ReplaceContent
    {
        public static string ReplaceImageUrls(string content, List<string> oldUrls, List<string> newUrls)
        {
            foreach (var newUrl in newUrls)
            {
                var urls = newUrl.Split('/');
                var nameImage = urls[urls.Length - 1];
                var urlsNeedToReplace = oldUrls.FirstOrDefault(x => x.Contains(nameImage));
                if (urlsNeedToReplace != null)
                {
                    content = content.Replace(urlsNeedToReplace, newUrl);
                }
            }

            return content;
        }
    }

    public enum WeatherType
    {
        Humidity = 1,   //DoAm
        WindLevel,      //GioGiat
        WindDirection,  //HuongGio
        WindSpeed,      //TocDoGio
        RainAmount,     //LuongMua
        Temperature,    //NhietDo
        Weather,        //ThoiTiet...
        WindRank        //CapGio
    }

    public enum HydrologicalForeCastType
    {
        Default = 1
    }

    public enum LookupType
    {
        Province = 1,
        District = 2,
        ActionType = 3,
        ActionMethod = 4,
        ActionAreaType = 5,
        ScenarioActionType = 6,
        Position = 7
    }

    public enum ScenarioActionType
    {
        CustomLocationControl = 1,
        CustomMapStatusControl = 2,
        CustomLevelControl = 3,
        CustomZoomControl = 4,
        CustomWaitControl = 5,
        CustomImportVideoControl = 6
    }

    public enum PostStatus
    {
        WaitingForApproval = 1,
        Public,
        Private,
        Blocked,
        Deleted
    }
}