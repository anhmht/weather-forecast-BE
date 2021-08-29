using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList
{
    public class ApplicationUserDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
