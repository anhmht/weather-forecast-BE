using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.UpdateWeatherState
{
    public class UpdateWeatherStateCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
