using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario
{
    public class UpdateScenarioCommandHandler : IRequestHandler<UpdateScenarioCommand>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IMapper _mapper;

        public UpdateScenarioCommandHandler(IScenarioRepository scenarioRepository, IMapper mapper)
        {
            _scenarioRepository = scenarioRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
        {
            var validatior = new UpdateScenarioCommandValidator();
            var validationResult = validatior.Validate(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var scenarioToUpdate = await _scenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenarioToUpdate == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            _mapper.Map(request, scenarioToUpdate, typeof(UpdateScenarioCommand), typeof(Scenario));

            await _scenarioRepository.UpdateAsync(scenarioToUpdate);
            
            return Unit.Value;

        }
    }
}