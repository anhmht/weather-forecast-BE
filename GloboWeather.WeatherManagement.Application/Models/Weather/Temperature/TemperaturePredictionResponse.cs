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
    public class TemperaturePredictionResponse
    {
        public string DiemId { get; set; }

        public List<TemperatureDayResponse> TemperatureByDays { get; set; }

        public int TemperatureMin { get; set; }

        public int TemperatureMax { get; set; }
    }
}
