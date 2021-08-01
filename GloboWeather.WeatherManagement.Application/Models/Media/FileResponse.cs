namespace GloboWeather.WeatherManagement.Application.Models.Media
{
    public record FileResponse
    {
        public string Url { get; init; }
    }

    public record ImageResponse : FileResponse
    {
    }

    public record DocumentResponse : FileResponse
    {
        public long ContentLength { get; init; }
    }
}