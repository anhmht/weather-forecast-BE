using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Weather
{
    public class NhietDoTheoNgayResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        public List<NhietDoTheoGio> NhietDoTheoGios { get; set; }
    }
}
