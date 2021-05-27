using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation
{
    public class GetLocationResponse
    {
        [JsonPropertyName("data")]
        public  List<PositionLocationVm> Location { get; set; }
    }
}