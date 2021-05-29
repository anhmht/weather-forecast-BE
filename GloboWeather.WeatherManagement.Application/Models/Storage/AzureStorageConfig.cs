namespace GloboWeather.WeatherManagement.Application.Models.Storage
{
    public class AzureStorageConfig
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ImageContainer { get; set; }
        public string ThumbnailContainer { get; set; }
        public string EventContainer { get; set; }
        public string TempContainer { get; set; }
    }
}