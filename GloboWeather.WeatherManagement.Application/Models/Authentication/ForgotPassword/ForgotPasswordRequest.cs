using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}