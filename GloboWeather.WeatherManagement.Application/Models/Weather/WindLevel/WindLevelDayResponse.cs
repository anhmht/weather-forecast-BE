using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class WindLevelDayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<WindLevelHour> WindLevelByHours { get; set; }

        public List<WindLevelHour> WindLevelMins { get; set; }

        public List<WindLevelHour> WindLevelMaxs { get; set; }

        public int WindLevelMin { get; set; }

        public int WindLevelMax { get; set; }

        public WindLevelDayResponse()
        {
            WindLevelByHours = new List<WindLevelHour>();
            WindLevelMins = new List<WindLevelHour>();
            WindLevelMaxs = new List<WindLevelHour>();
        }
    }
}
