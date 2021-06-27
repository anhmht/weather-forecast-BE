using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList
{
    public class UserListVm
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}