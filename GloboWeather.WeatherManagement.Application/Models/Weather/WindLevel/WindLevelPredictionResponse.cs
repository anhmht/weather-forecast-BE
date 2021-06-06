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
    public class WindLevelPredictionResponse
    {
        public string DiemId { get; set; }

        public List<WindLevelDayResponse> WindLevelByDays { get; set; }

        public int WindLevelMin { get; set; }

        public int WindLevelMax { get; set; }
    }
}
