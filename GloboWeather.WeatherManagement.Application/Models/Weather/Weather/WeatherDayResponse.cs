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

        public List<WeatherHour> WeatherMins { get; set; }

        public List<WeatherHour> WeatherMaxs { get; set; }

        public int WeatherMin { get; set; }

        public int WeatherMax { get; set; }

        public WeatherDayResponse()
        {
            WeatherByHours = new List<WeatherHour>();
            WeatherMins = new List<WeatherHour>();
            WeatherMaxs = new List<WeatherHour>();
        }
    }
}
