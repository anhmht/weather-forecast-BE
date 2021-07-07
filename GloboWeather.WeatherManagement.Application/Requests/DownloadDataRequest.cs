using System;

namespace GloboWeather.WeatherManagement.Application.Requests
{
    public class DownloadDataRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
