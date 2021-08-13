using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Responses
{
    public class BasePagingResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
