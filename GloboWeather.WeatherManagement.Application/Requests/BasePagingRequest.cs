namespace GloboWeather.WeatherManagement.Application.Requests
{
    public class BasePagingRequest
    {
        public int Limit { get; set; }
        public int Page { get; set; }
    }
}
