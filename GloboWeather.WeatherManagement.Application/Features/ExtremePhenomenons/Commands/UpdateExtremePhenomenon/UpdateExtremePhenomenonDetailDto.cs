using System;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon
{
    public class UpdateExtremePhenomenonDetailDto
    {
        public Guid Id { get; set; }
        public Guid ExtremePhenomenonId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
