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
    public class RainAmountPredictionResponse
    {
        public string DiemId { get; set; }

        public List<RainAmountDayResponse> RainAmountByDays { get; set; }

        public int RainAmountMin { get; set; }

        public int RainAmountMax { get; set; }
    }
}
