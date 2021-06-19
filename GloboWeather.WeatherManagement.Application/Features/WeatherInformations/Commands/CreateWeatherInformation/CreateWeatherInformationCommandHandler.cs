using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation
{
    public class CreateWeatherInformationCommandHandler : IRequestHandler<CreateWeatherInformationCommand, Guid>
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWeatherInformationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            _unitOfWork.WeatherInformationRepository.Add(@WeatherInfomation);
            await _unitOfWork.CommitAsync();
            return @WeatherInfomation.ID;
        }

    }
}