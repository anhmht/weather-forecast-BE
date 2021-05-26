using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Models.Astronomy
{
    public class GetAstronomyResponse
    {
        [JsonPropertyName("location")]
        public LocationVm Location { get; set; }
        
        [JsonPropertyName("astronomy")]
        public  AstronomyVm Astro { get; set; }
    }
}