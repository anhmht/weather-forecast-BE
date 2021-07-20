using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario
{
    public class CreateScenarioCommandHandler : IRequestHandler<CreateScenarioCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateScenarioCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateScenarioCommand request, CancellationToken cancellationToken)
        {
            var validatior = new CreateScenarioCommandValidator();

            var validationResult = validatior.Validate(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var @scenario = _mapper.Map<Scenario>(request);
            if (@scenario.ScenarioContent == null)
                @scenario.ScenarioContent = string.Empty;
            _unitOfWork.ScenarioRepository.Add(@scenario);
            await _unitOfWork.CommitAsync();

            return @scenario.ScenarioId;

        }
    }
}