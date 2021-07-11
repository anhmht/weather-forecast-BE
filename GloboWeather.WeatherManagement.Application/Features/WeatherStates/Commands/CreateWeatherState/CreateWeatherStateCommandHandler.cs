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

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState
{
    public class CreateWeatherStateCommandHandler : IRequestHandler<CreateWeatherStateCommand, Guid>
    {
     
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWeatherStateCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        
        public async Task<Guid> Handle(CreateWeatherStateCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateWeatherStateCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var weatherState = _mapper.Map<WeatherState>(request);
            weatherState.Id = Guid.NewGuid();
            if (request.ImageFile != null)
            {
                var imageResponse = await _imageService.UploadImageAsync(request.ImageFile);
                if (!string.IsNullOrEmpty(imageResponse.Url))
                {
                    weatherState.ImageUrl = (await _imageService.CopyImageToEventPost(
                        new List<string> {imageResponse.Url},
                        weatherState.Id.ToString(), Forder.FeatureImage)).FirstOrDefault();
                }
            }

            _unitOfWork.WeatherStateRepository.Add(weatherState);
            await _unitOfWork.CommitAsync();

            return  weatherState.Id;
        }

    }
}