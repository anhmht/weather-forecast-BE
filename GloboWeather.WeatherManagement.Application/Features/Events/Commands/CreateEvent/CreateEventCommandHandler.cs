using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
     
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        
        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateEventCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var @event = _mapper.Map<Event>(request);
            @event.EventId = Guid.NewGuid();

            if (request.ImageNormalUrls?.Any() == true)
            {
                //UpLoad to Normal Image
                var imageUrlsListAfterUpdate = await _imageService.CopyImageToEventPost(request.ImageNormalUrls, @event.EventId.ToString(), Forder.NormalImage);
                @event.Content = ReplaceContent.ReplaceImageUrls(request.Content, request.ImageNormalUrls, imageUrlsListAfterUpdate);
            }

            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                //Upload to Feature Image
                @event.ImageUrl = (await _imageService.CopyImageToEventPost(new List<string> {request.ImageUrl},
                    @event.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
            }

            _unitOfWork.EventRepository.Add(@event);

            await CreateDocumentAsync(request, @event);

            await _unitOfWork.CommitAsync();
            
            return  @event.EventId;
        }

        private async Task CreateDocumentAsync(CreateEventCommand request, Event @event)
        {
            if (request.Documents?.Any() == true)
            {
                foreach (var document in request.Documents)
                {
                    var documentUrl = (await _imageService.CopyImageToEventPost(new List<string> {document.Url},
                        @event.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
                    _unitOfWork.EventDocumentRepository.Add(new EventDocument()
                    {
                        Name = document.Name,
                        EventId = @event.EventId,
                        Id = Guid.NewGuid(),
                        Url = documentUrl,
                        ContentLength = document.ContentLength
                    });
                }
            }
        }
    }
}