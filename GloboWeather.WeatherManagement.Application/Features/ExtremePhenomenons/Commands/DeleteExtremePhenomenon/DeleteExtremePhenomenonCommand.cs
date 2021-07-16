using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.DeleteExtremePhenomenon
{
    public class DeleteExtremePhenomenonCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
