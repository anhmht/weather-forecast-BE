using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological
{
    public class GetHydrologicalListQuery
    {
        public  int Limit { get; set; }
        public  int Page { get; set; }
       
        public string StationId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}