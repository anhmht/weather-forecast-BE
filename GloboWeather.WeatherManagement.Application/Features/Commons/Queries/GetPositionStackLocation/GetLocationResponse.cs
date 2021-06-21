using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation
{
    public class GetLocationResponse
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }
        
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
        
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }
        
        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; }
        
        [JsonPropertyName("region_name")]
        public string RegionName { get; set; }
    }
}