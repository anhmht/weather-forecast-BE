using System;
using Microsoft.AspNetCore.Identity;

namespace GloboWeather.WeatherManagement.Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string AvatarUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
    
    
}