using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario
{
    public class UpdateScenarioCommandHandler : IRequestHandler<UpdateScenarioCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateScenarioCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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

            var scenarioToUpdate = await _unitOfWork.ScenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenarioToUpdate == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            _mapper.Map(request, scenarioToUpdate, typeof(UpdateScenarioCommand), typeof(Scenario));
            if (scenarioToUpdate.ScenarioContent == null)
                scenarioToUpdate.ScenarioContent = string.Empty;
            _unitOfWork.ScenarioRepository.Update(scenarioToUpdate);
            await _unitOfWork.CommitAsync();

            return Unit.Value;

        }
    }
}