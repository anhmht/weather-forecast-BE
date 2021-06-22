using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication
{
    public class UploadAvatarRequest
    {
        public string UserId { get; set; }
        public IFormFile Image { get; set; }
    }
}