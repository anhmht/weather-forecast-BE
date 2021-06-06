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

        public List<TemperatureByHour> TemperatureByHours { get; set; }

        public List<TemperatureByHour> TemperatureMins { get; set; }

        public List<TemperatureByHour> TemperatureMaxs { get; set; }

        public int TemperatureMin { get; set; }

        public int TemperatureMax { get; set; }

        public TemperatureDayResponse()
        {
            TemperatureByHours = new List<TemperatureByHour>();
            TemperatureMins = new List<TemperatureByHour>();
            TemperatureMaxs = new List<TemperatureByHour>();
        }
    }
}
