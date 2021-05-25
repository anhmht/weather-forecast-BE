using System;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusDto
    {
        public Guid StatusId { get; set; }
        public string Name { get; set; }
    }
}