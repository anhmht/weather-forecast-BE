using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication
{
    public class ForgotPasswordResponse : BaseResponse
    {
        public  string UserId { get; set; }
        public  string Code { get; set; }
    }
}