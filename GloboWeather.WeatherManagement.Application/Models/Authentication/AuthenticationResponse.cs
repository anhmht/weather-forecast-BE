using GloboWeather.WeatherManegement.Application.Responses;

namespace GloboWeather.WeatherManegement.Application.Models.Authentication
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvartarUrl { get; set; }
    }
}