using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest
    {
        public Guid EventId { get; set; }
    }
}