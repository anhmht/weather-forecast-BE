using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteEventCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId);
            if (eventToDelete == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            await _imageService.DeleteImagesInPostsContainerAsync(request.EventId.ToString());

            _unitOfWork.EventRepository.Delete(eventToDelete);
            await _unitOfWork.CommitAsync();

            return  Unit.Value;
        }
    }
}