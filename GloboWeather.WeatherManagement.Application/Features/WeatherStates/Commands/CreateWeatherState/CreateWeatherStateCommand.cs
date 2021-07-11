using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState
{
    public class CreateWeatherStateCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
