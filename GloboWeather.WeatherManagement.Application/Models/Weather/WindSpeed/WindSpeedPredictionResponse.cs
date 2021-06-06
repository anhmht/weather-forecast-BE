using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    /// <summary>
    /// get from table NhietDo, then format data to list by diemId
    /// </summary>
    public class WindSpeedPredictionResponse
    {
        public string DiemId { get; set; }

        public List<WindSpeedDayResponse> WindSpeedByDays { get; set; }

        public int WindSpeedMin { get; set; }

        public int WindSpeedMax { get; set; }
    }
}
