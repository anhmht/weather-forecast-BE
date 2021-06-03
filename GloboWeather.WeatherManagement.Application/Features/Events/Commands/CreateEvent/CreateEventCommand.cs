using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommand:IRequest<Guid>
    {
        public string Title { get; set; }
        public  string Content { get; set; }
        public  string ImageUrl { get; set; }
        public  List<string> NormalImageUrls { get; set; }
        public  DateTime DatePosted { get; set; }
        public  Guid CategoryId { get; set; }
        public  Guid StatusId { get; set; }
    }
}