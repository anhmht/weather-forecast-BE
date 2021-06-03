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
using Microsoft.EntityFrameworkCore.Internal;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>

    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IImageService _imageService;

        public UpdateEventCommandHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IImageService imageService)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _imageService = imageService;
        }
        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);
            var imageListUrlsNeedToDelete = new List<string>();
            imageListUrlsNeedToDelete.AddRange(request.ImageNormalDeletes);
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
            
            //await _imageService.CopyImageToEventPost(request.ImageNormalAdd,request.EventId.ToString(), Forder.NormalImage);

            if (string.Compare(request.ImageUrl, eventToUpdate.ImageUrl) != 0)
            {
               imageListUrlsNeedToDelete.Add(eventToUpdate.ImageUrl);
                request.ImageUrl = (await _imageService.CopyImageToEventPost(new List<string> {request.ImageUrl},
                    request.EventId.ToString(), Forder.FeatureImage)).FirstOrDefault();
            }

            await _imageService.CopyImageToEventPost(request.ImageNormalAdds, request.EventId.ToString(),
                Forder.NormalImage);
            await _imageService.DeleteImagesInPostsContainerByNameAsync(request.EventId.ToString(),
                imageListUrlsNeedToDelete);
            
            //Upload to Feature Image
         
            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

            await _eventRepository.UpdateAsync(eventToUpdate);
            
            return Unit.Value;
        }
    }
}