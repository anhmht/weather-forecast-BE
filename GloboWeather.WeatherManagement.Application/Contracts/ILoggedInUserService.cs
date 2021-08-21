namespace GloboWeather.WeatherManegement.Application.Contracts
{
    public interface ILoggedInUserService
    {
        public string UserId { get; }
        public string IpAddress { get; }
    }
}