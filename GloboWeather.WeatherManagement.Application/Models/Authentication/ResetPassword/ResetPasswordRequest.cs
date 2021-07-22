using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.ResetPassword
{
    public class ResetPasswordRequest
    {
        [Required]
        public string UserId { get ; set; }
        
        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        public string Code { get; set; }
    }
}