using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication
{
    public class UpdatingRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public List<string> RoleNames { get; set; }
    }
}