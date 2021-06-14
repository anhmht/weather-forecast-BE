using GloboWeather.WeatherManagement.Application.Helpers.Common;
using System;
using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationResponse
    {
        public List<WeatherInformationByStation> WeatherInformationByStations { get; set; }

        public GetWeatherInformationResponse()
        {
            WeatherInformationByStations = new List<WeatherInformationByStation>();
        }
    }

    public class WeatherInformationByStation
    {
        public string StationId { get; set; }
        public WeatherType WeatherType { get; set; }

        public List<WeatherInformationByDay> WeatherInformationByDays { get; set; }

        public int? MinValue { get; set; }

        public int? MaxValue { get; set; }

        public WeatherInformationByStation()
        {
            WeatherInformationByDays = new List<WeatherInformationByDay>();
        }
    }

    public class WeatherInformationByDay
    {
        public DateTime Date { get; set; }

        public List<WeatherInformationByHour> WeatherInformationByHours { get; set; }

        public List<WeatherInformationByHour> WeatherInformationMins { get; set; }

        public List<WeatherInformationByHour> WeatherInformationMaxs { get; set; }

        public int? MinValue { get; set; }

        public int? MaxValue { get; set; }

        public WeatherInformationByDay()
        {
            WeatherInformationByHours = new List<WeatherInformationByHour>();
            WeatherInformationMins = new List<WeatherInformationByHour>();
            WeatherInformationMaxs = new List<WeatherInformationByHour>();
        }
    }

    public class WeatherInformationByHour
    {
        public int Hour { get; set; }

        public object Value { get; set; }
    }
}
