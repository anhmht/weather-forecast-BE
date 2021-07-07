namespace GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import
{
    public class ImportHydrologicalForeCastDto
    {
        public string StationId { get; set; }
        public string RefDate { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
    }
}
