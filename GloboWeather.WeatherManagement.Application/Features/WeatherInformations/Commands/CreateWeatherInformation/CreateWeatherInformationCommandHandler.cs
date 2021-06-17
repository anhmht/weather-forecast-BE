using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation
{
    public class CreateWeatherInformationCommandHandler : IRequestHandler<CreateWeatherInformationCommand, Guid>
    {

        private readonly IMapper _mapper;
        private readonly IWeatherInformationRepository _weatherInfomationRepository;

        public CreateWeatherInformationCommandHandler(IMapper mapper, IWeatherInformationRepository WeatherInfomationRepository)
        {
            _mapper = mapper;
            _weatherInfomationRepository = WeatherInfomationRepository;
        }

        public async Task<Guid> Handle(CreateWeatherInformationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateWeatherInformationCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var @WeatherInfomation = _mapper.Map<WeatherInformation>(request);
            @WeatherInfomation = await _weatherInfomationRepository.AddAsync(@WeatherInfomation);

            return @WeatherInfomation.ID;
        }

    }
}