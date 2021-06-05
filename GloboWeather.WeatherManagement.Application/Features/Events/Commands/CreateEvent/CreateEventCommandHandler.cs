using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
     
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;

        public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IImageService imageService)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _imageService = imageService;
        }
        
        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateEventCommandValidator(_eventRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var @event = _mapper.Map<Event>(request);
            @event.EventId = Guid.NewGuid();
          
            //UpLoad to Normal Image
           var imageUrlsListAfterUpdate = await _imageService.CopyImageToEventPost(request.ImageNormalUrls, @event.EventId.ToString(), Forder.NormalImage);
           
            //Upload to Feature Image
            @event.ImageUrl = (await _imageService.CopyImageToEventPost(new List<string> {request.ImageUrl},
                @event.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
            @event.Content = ReplaceContent.ReplaceImageUrls(request.Content, request.ImageNormalUrls, imageUrlsListAfterUpdate);
            
            @event = await _eventRepository.AddAsync(@event);
            
            return  @event.EventId;
        }

    }
}