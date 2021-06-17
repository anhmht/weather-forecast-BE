using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation
{
    public class PositionLocationVm
    {
        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }
        
        [JsonPropertyName("longitude")]
        public  decimal Longitude { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("distance")]
        public decimal Distance { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("confidence")]
        public decimal Confidence { get; set; }
        
        [JsonPropertyName("region")]
        public  string Region { get; set; }
        
        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; }
        
        [JsonPropertyName("county")]
        public string County { get; set; }
        
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
        
        [JsonPropertyName("continent")]
        public string Continent { get; set; }
        
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}