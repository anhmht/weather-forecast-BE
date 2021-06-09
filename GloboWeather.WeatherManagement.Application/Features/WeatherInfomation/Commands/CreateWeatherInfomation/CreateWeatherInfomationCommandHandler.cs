using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInfomations.Commands.CreateWeatherInfomation
{
    public class CreateWeatherInfomationCommandHandler : IRequestHandler<CreateWeatherInfomationCommand, Guid>
    {
     
        private readonly IMapper _mapper;
        private readonly IWeatherInfomationRepository _WeatherInfomationRepository;
        private readonly IImageService _imageService;

        public CreateWeatherInfomationCommandHandler(IMapper mapper, IWeatherInfomationRepository WeatherInfomationRepository, IImageService imageService)
        {
            _mapper = mapper;
            _WeatherInfomationRepository = WeatherInfomationRepository;
            _imageService = imageService;
        }
        
        public async Task<Guid> Handle(CreateWeatherInfomationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateWeatherInfomationCommandValidator(_WeatherInfomationRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var @WeatherInfomation = _mapper.Map<WeatherInfomation>(request);
            @WeatherInfomation.WeatherInfomationId = Guid.NewGuid();

            if (request.ImageNormalUrls.Any())
            {
                //UpLoad to Normal Image
                var imageUrlsListAfterUpdate = await _imageService.CopyImageToWeatherInfomationPost(request.ImageNormalUrls, @WeatherInfomation.WeatherInfomationId.ToString(), Forder.NormalImage);
                @WeatherInfomation.Content = ReplaceContent.ReplaceImageUrls(request.Content, request.ImageNormalUrls, imageUrlsListAfterUpdate);
            }

            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                //Upload to Feature Image
                @WeatherInfomation.ImageUrl = (await _imageService.CopyImageToWeatherInfomationPost(new List<string> {request.ImageUrl},
                    @WeatherInfomation.WeatherInfomationId.ToString(), Forder.FeatureImage)).FirstOrDefault();
            }
           
            @WeatherInfomation = await _WeatherInfomationRepository.AddAsync(@WeatherInfomation);
            
            return  @WeatherInfomation.WeatherInfomationId;
        }

    }
}