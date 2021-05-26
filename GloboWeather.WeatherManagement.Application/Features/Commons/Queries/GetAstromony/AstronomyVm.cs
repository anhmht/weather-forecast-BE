using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Models.Astronomy
{
    public class AstronomyVm
    {
        [JsonPropertyName("astro")]
        public AstroVm Astro { get; set; }
          
    }

    public class AstroVm
    {
        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; }
        
        [JsonPropertyName("sunset")]
        public string Sunset { get; set; }
        
        [JsonPropertyName("moonrise")]
        public string Moonrise { get; set; }
        
        [JsonPropertyName("moon_phase")]
        public string MoonPhase { get; set; }
        
        [JsonPropertyName("moon_illumination")]
        public decimal MoonIllumination { get; set; }
    }
}