using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class WeatherDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<WeatherHour> WeatherByHours { get; set; }       

        public WeatherDayResponse()
        {
            WeatherByHours = new List<WeatherHour>();         
        }
    }
}
