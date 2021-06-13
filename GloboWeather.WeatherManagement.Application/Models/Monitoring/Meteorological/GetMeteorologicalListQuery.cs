using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical
{
    public class GetMeteorologicalListQuery
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
        public IEnumerable<int> ZipCodes { get; set; }
        
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}