using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class WindSpeedDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<WindSpeedHour> WindSpeedByHours { get; set; }

        public List<WindSpeedHour> WindSpeedMins { get; set; }

        public List<WindSpeedHour> WindSpeedMaxs { get; set; }

        public int WindSpeedMin { get; set; }

        public int WindSpeedMax { get; set; }

        public WindSpeedDayResponse()
        {
            WindSpeedByHours = new List<WindSpeedHour>();
            WindSpeedMins = new List<WindSpeedHour>();
            WindSpeedMaxs = new List<WindSpeedHour>();
        }
    }
}
