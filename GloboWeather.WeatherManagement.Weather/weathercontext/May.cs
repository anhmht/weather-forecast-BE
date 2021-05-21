using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Weather.weathercontext
{
    public partial class May
    {
        public int Id { get; set; }
        public string MoTa { get; set; }
        public byte? _24h { get; set; }
        public byte? Ngay { get; set; }
        public byte? Dem { get; set; }
    }
}
