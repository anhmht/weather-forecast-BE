using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.DeleteExtremePhenomenon
{
    public class DeleteExtremePhenomenonCommandHandler : IRequestHandler<DeleteExtremePhenomenonCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExtremePhenomenonCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteExtremePhenomenonCommand request, CancellationToken cancellationToken)
        {
            var extremePhenomenon = await _unitOfWork.ExtremePhenomenonRepository.GetByIdAsync(request.Id);
            if (extremePhenomenon == null)
            {
                throw new NotFoundException(nameof(extremePhenomenon), request.Id);
            }

            _unitOfWork.ExtremePhenomenonDetailRepository.DeleteWhere(
                x => x.ExtremePhenomenonId == extremePhenomenon.Id);
            _unitOfWork.ExtremePhenomenonRepository.Delete(extremePhenomenon);
            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}