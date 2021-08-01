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
        public DateTime? CreatedOn { get; set; }
        public string ShortName => $"{LastName?[0]}{FirstName?[0]}";
        public bool? IsActive { get; set; }
        public string Status => IsActive == true ? "Đang sử dụng" : "Khóa";
    }
}