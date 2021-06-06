using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class HumidityDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<HumidityHour> HumidityByHours { get; set; }

        public List<HumidityHour> HumidityMins { get; set; }

        public List<HumidityHour> HumidityMaxs { get; set; }

        public int HumidityMin { get; set; }

        public int HumidityMax { get; set; }

        public HumidityDayResponse()
        {
            HumidityByHours = new List<HumidityHour>();
            HumidityMins = new List<HumidityHour>();
            HumidityMaxs = new List<HumidityHour>();
        }
    }
}
