using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest
{
    public class CreateUserResponse : BaseResponse
    {
        public string UserId { get; set; }
    }
}