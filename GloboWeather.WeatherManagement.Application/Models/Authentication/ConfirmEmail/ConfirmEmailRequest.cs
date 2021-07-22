namespace GloboWeather.WeatherManagement.Application.Models.Authentication.ConfirmEmail
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        
    }
}