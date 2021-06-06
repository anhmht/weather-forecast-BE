using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class RainAmountDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<RainAmountHour> RainAmountByHours { get; set; }

        public List<RainAmountHour> RainAmountMins { get; set; }

        public List<RainAmountHour> RainAmountMaxs { get; set; }

        public int RainAmountMin { get; set; }

        public int RainAmountMax { get; set; }

        public RainAmountDayResponse()
        {
            RainAmountByHours = new List<RainAmountHour>();
            RainAmountMins = new List<RainAmountHour>();
            RainAmountMaxs = new List<RainAmountHour>();
        }
    }
}
