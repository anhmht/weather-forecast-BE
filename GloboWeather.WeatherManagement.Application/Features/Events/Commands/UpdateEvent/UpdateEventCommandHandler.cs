using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>

    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;

        public UpdateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IImageService imageService)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _imageService = imageService;
        }
        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);
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

            if (request.ImageNormalAdds.Any())
            {
                List<string> imageUrlsAfterUpload =  await _imageService.CopyImageToEventPost(request.ImageNormalAdds, request.EventId.ToString(),
                    Forder.NormalImage);
                request.Content =
                    ReplaceContent.ReplaceImageUrls(request.Content, request.ImageNormalAdds, imageUrlsAfterUpload);
            }

            if (request.ImageNormalDeletes.Any())
            {
                imageListUrlsNeedToDelete.AddRange(request.ImageNormalDeletes);
            }
            
            if (imageListUrlsNeedToDelete.Any() )
            {
             
                await _imageService.DeleteImagesInPostsContainerByNameAsync(request.EventId.ToString(),
                    imageListUrlsNeedToDelete);
            }
            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

            await _eventRepository.UpdateAsync(eventToUpdate);
            
            return Unit.Value;
        }
    }
}