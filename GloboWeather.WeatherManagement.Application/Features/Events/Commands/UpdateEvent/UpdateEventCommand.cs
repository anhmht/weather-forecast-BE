using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public  string Content { get; set; }
        public  string ImageUrl { get; set; }
        public  DateTime DatePosted { get; set; }
        public  Guid CategoryId { get; set; }
        public  Guid StatusId { get; set; }
        public  List<string> ImageNormalDeletes { get; set; }
        public List<string> ImageNormalAdds { get; set; }
        public List<UpdateDocuments> Documents { get; set; }
    }

    public class UpdateDocuments
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public long ContentLength { get; set; }
    }
}