using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateActionOrder
{
    public class UpdateActionOrderCommand : IRequest
    {
        public List<ActionOrderCommand> ActionOrders { get; set; }
    }

    public class ActionOrderCommand
    {
        public Guid ActionId { get; set; }
        public int Order { get; set; }
    }
}
