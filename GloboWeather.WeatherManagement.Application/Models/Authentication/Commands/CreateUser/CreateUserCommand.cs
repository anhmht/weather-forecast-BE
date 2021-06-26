using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest
{
    public class CreateUserCommand
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public  string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        public List<string> RoleNames { get; set; }
        
        public string AvatarUrl { get; set; }
        
    }
}