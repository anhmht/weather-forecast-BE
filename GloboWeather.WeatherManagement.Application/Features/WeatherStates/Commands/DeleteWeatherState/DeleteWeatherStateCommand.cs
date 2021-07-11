using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.DeleteWeatherState
{
    public class DeleteWeatherStateCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
