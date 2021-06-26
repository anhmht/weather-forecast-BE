using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList
{
    public class GetUserListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<UserListVm> Users { get; set; }
    }
}