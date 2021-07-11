using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.UpdateWeatherState
{
    public class UpdateWeatherStateCommandHandler : IRequestHandler<UpdateWeatherStateCommand, Guid>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWeatherStateCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<Guid> Handle(UpdateWeatherStateCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateWeatherStateCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult);
            }

            var weatherState = await _unitOfWork.WeatherStateRepository.GetByIdAsync(request.Id);
            if (weatherState == null)
            {
                throw new NotFoundException(nameof(WeatherState), request.Id);
            }

            var isUpdate = false;
            if (request.ImageFile != null)
            {
                var imageResponse = await _imageService.UploadImageAsync(request.ImageFile);
                if (!string.IsNullOrEmpty(imageResponse.Url))
                {
                    weatherState.ImageUrl = (await _imageService.CopyImageToEventPost(
                        new List<string> { imageResponse.Url },
                        weatherState.Id.ToString(), Forder.FeatureImage)).FirstOrDefault();
                    isUpdate = true;
                }
            }

            if (!request.Name.Equals(weatherState.Name))
            {
                weatherState.Name = request.Name;
                isUpdate = true;
            }

            if (!request.Content.Equals(weatherState.Content))
            {
                weatherState.Content = request.Content;
                isUpdate = true;
            }

            if (isUpdate)
            {
                _unitOfWork.WeatherStateRepository.Update(weatherState);
                await _unitOfWork.CommitAsync();
            }

            return weatherState.Id;
        }

    }
}