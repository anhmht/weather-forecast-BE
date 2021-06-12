using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInfomation
{
    public class ImportWeatherInformationCommandHandler : IRequestHandler<CreateWeatherInformationCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IWeatherInformationRepository _WeatherInfomationRepository;
        private readonly IImageService _imageService;

        public ImportWeatherInformationCommandHandler(IMapper mapper, IWeatherInformationRepository WeatherInfomationRepository, IImageService imageService)
        {
            _mapper = mapper;
            _WeatherInfomationRepository = WeatherInfomationRepository;
            _imageService = imageService;
        }
        
        public async Task<Guid> Handle(CreateWeatherInformationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateWeatherInformationCommandValidator(_WeatherInfomationRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var @WeatherInfomation = _mapper.Map<WeatherInformation>(request);
            @WeatherInfomation.ID = Guid.NewGuid();

           
            @WeatherInfomation = await _WeatherInfomationRepository.AddAsync(@WeatherInfomation);
            
            return  @WeatherInfomation.ID;
        }

    }
}