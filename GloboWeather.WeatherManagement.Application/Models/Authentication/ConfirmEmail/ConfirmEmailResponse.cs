using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.ConfirmEmail
{
    public class ConfirmEmailResponse : BaseResponse
    {
        public string UserId { get; set; }
    }
}