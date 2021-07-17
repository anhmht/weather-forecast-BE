using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>

    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UpdateEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId);
            var imageListUrlsNeedToDelete = new List<string>();
           // imageListUrlsNeedToDelete.AddRange(request.ImageNormalDeletes);
            if (eventToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            var validator = new UpdateEventCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new ValidationException(validationResult);
            }
            if (string.Compare(request.ImageUrl, eventToUpdate.ImageUrl) != 0 
                    && !string.IsNullOrEmpty(request.ImageUrl))
            {
               imageListUrlsNeedToDelete.Add(eventToUpdate.ImageUrl);
                request.ImageUrl = (await _imageService.CopyImageToEventPost(new List<string> {request.ImageUrl},
                    request.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
            }

            if (request.ImageNormalAdds?.Any() == true)
            {
                List<string> imageUrlsAfterUpload =  await _imageService.CopyImageToEventPost(request.ImageNormalAdds, request.EventId.ToString(),
                    Forder.NormalImage);
                request.Content =
                    ReplaceContent.ReplaceImageUrls(request.Content, request.ImageNormalAdds, imageUrlsAfterUpload);
            }

            if (request.ImageNormalDeletes?.Any() == true)
            {
                imageListUrlsNeedToDelete.AddRange(request.ImageNormalDeletes);
            }
            
            if (imageListUrlsNeedToDelete.Any())
            {
             
                await _imageService.DeleteImagesInPostsContainerByNameAsync(request.EventId.ToString(),
                    imageListUrlsNeedToDelete);
            }
            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

            _unitOfWork.EventRepository.Update(eventToUpdate);

            await UpdateDocumentAsync(request, eventToUpdate);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }

        private async Task UpdateDocumentAsync(UpdateEventCommand request, Event eventToUpdate)
        {
            var eventDocuments = await _unitOfWork.EventDocumentRepository.GetByEventIdAsync(eventToUpdate.EventId);
            foreach (var eventDocument in eventDocuments)
            {
                var isUpdate = false;
                var entry = request.Documents?.FirstOrDefault(x => x.Id == eventDocument.Id);
                if (entry != null) //Update document
                {
                    if (!eventDocument.Name.Equals(entry.Name))
                    {
                        eventDocument.Name = entry.Name;
                        isUpdate = true;
                    }

                    if (!string.IsNullOrEmpty(entry.Url) && !eventDocument.Url.Equals(entry.Url))
                    {
                        //Delete old file
                        await _imageService.DeleteImagesInPostsContainerByNameAsync(eventToUpdate.EventId.ToString(),
                            new List<string> {eventDocument.Url});

                        //Copy new file from temp to event folder
                        var documentUrl = (await _imageService.CopyImageToEventPost(new List<string> {entry.Url},
                            eventToUpdate.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
                        eventDocument.Url = documentUrl;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.EventDocumentRepository.Update(eventDocument);
                    }
                }
                else //Delete document
                {
                    _unitOfWork.EventDocumentRepository.Delete(eventDocument);
                }
            }

            if (request.Documents != null) //Add new document
            {
                foreach (var insertDocument in request.Documents.FindAll(x => x.Id.Equals(Guid.Empty)))
                {
                    var documentUrl = (await _imageService.CopyImageToEventPost(new List<string> {insertDocument.Url},
                        eventToUpdate.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();

                    _unitOfWork.EventDocumentRepository.Add(new EventDocument()
                    {
                        Name = insertDocument.Name,
                        EventId = eventToUpdate.EventId,
                        Id = Guid.NewGuid(),
                        Url = documentUrl
                    });
                }
            }
        }
    }
}