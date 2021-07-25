using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.ChangePassword
{
    public class ChangePasswordRequest
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password ang confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        
        public  string Token { get; set; }
    }
}