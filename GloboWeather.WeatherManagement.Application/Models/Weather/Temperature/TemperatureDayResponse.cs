using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class TemperatureDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<TemperatureHour> TemperatureHours { get; set; }

        public List<TemperatureHour> TemperatureMins { get; set; }

        public List<TemperatureHour> TemperatureMaxs { get; set; }

        public int TemperatureMin { get; set; }

        public int TemperatureMax { get; set; }

        public TemperatureDayResponse()
        {
            TemperatureHours = new List<TemperatureHour>();
            TemperatureMins = new List<TemperatureHour>();
            TemperatureMaxs = new List<TemperatureHour>();
        }
    }
}
