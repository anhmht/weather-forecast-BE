using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList
{
    public class GetUsersListQuery
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
        public List<string> RoleIds { get; set; }
    }
}