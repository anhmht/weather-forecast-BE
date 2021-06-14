using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario
{
    public class CreateScenarioCommandHandler : IRequestHandler<CreateScenarioCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IScenarioRepository _scenarioRepository;

        public CreateScenarioCommandHandler(IMapper mapper, IScenarioRepository scenarioRepository)
        {
            _mapper = mapper;
            _scenarioRepository = scenarioRepository;
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

            @scenario = await _scenarioRepository.AddAsync(@scenario);

            return @scenario.ScenarioId;

        }
    }
}