using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.DeleteWeatherState
{
    public class DeleteWeatherStateCommandHandler : IRequestHandler<DeleteWeatherStateCommand>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWeatherStateCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(DeleteWeatherStateCommand request, CancellationToken cancellationToken)
        {
            var weatherState = await _unitOfWork.WeatherStateRepository.GetByIdAsync(request.Id);
            if (weatherState == null)
            {
                throw new NotFoundException(nameof(WeatherState), request.Id);
            }

            await _imageService.DeleteImagesInPostsContainerAsync(weatherState.Id.ToString());

            _unitOfWork.WeatherStateRepository.Delete(weatherState);
            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}