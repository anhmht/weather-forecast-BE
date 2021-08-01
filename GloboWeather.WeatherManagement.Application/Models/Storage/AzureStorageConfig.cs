namespace GloboWeather.WeatherManagement.Application.Models.Storage
{
    public class AzureStorageConfig
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        
        public string TempContainer { get; set; }
        public string PostContainer { get; set; }
        public string UserContainer { get; set; }
        public string ConnectionString { get; set; }
        public string Url { get; set; }
    }
}