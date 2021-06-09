using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInfomations.Commands.CreateWeatherInfomation
{
    public class CreateWeatherInfomationCommand : IRequest<Guid>
    {
        public string StationId { get; set; }
        public DateTime RefDate { get; set; }
        public string Humidity { get; set; }
        public string WindLevel { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public string RainAmount { get; set; }
        public string Temperature { get; set; }
        public string Weather { get; set; }
    }
}