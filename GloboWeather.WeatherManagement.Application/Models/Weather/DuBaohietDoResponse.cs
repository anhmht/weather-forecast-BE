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
    public class DuBaohietDoResponse
    {
        public string DiemId { get; set; }

        public List<NhietDoTheoNgayResponse> NhietDoTheoNgays { get; set; }

        public List<NhietDoTheoThoiGian> NhietDoMins { get; set; }

        public List<NhietDoTheoThoiGian> NhietDoMaxs { get; set; }

        public int NhietDoMin { get; set; }

        public int NhietDoMax { get; set; }
    }
}
