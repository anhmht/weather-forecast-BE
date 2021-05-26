using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Models.Astronomy
{
    public class LocationVm
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("region")]
        public string Region { get; set; }
        
        [JsonPropertyName("country")]
        public string Country { get; set; }
        
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }
        
        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }
        
        [JsonPropertyName("tz_id")]
        public string TzId { get; set; }
        
        [JsonPropertyName("localtime_epoch")]
        public int LocalTimeEPoch { get; set; }
        
        [JsonPropertyName("localtime")]
        public string LocalTime { get; set; }
    }
}