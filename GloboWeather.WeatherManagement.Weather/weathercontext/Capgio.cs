using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Weather.weathercontext
{
    public partial class Capgio
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? WindMS { get; set; }
        public float? WaveM { get; set; }
        public int? Color { get; set; }
    }
}
