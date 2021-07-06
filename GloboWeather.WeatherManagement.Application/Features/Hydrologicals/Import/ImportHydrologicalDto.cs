using System;

namespace GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import
{
    public class ImportHydrologicalDto
    {
        public string StationId { get; set; }
        public string Date { get; set; }
        public string Rain { get; set; }
        public string WaterLevel { get; set; }
        public string Accumulated { get; set; }
    }
}
