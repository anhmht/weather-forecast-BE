using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario
{
    public class DeleteScenarioCommandHandler : IRequestHandler<DeleteScenarioCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteScenarioCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Unit> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenarioToDelete = await _unitOfWork.ScenarioRepository.GetByIdAsync(request.ScenarioId);

            if (scenarioToDelete == null)
            {
                throw new NotFoundException(nameof(Scenario), request.ScenarioId);
            }

            _unitOfWork.ScenarioRepository.Delete(scenarioToDelete);
            await _unitOfWork.CommitAsync();

            return  Unit.Value;
            
        }
    }
}