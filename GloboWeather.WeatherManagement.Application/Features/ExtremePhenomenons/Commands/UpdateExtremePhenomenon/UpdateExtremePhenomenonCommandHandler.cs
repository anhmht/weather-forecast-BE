using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.UpdateExtremePhenomenon
{
    public class UpdateExtremePhenomenonCommandHandler : IRequestHandler<UpdateExtremePhenomenonCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExtremePhenomenonCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateExtremePhenomenonCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateExtremePhenomenonCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult);
            }

            var extremePhenomenon = await _unitOfWork.ExtremePhenomenonRepository.GetByIdAsync(request.Id);
            if (extremePhenomenon == null)
            {
                throw new NotFoundException(nameof(extremePhenomenon), request.Id);
            }

            var isUpdate = false;
            
            if (!request.ProvinceId.Equals(extremePhenomenon.ProvinceId))
            {
                extremePhenomenon.ProvinceId = request.ProvinceId;
                isUpdate = true;
            }

            if (!request.DistrictId.Equals(extremePhenomenon.DistrictId))
            {
                extremePhenomenon.DistrictId = request.DistrictId;
                isUpdate = true;
            }

            if (!request.Date.Date.Equals(extremePhenomenon.Date.Date))
            {
                extremePhenomenon.Date = request.Date.Date;
                isUpdate = true;
            }

            if (isUpdate)
            {
                _unitOfWork.ExtremePhenomenonRepository.Update(extremePhenomenon);
            }

            var extremePhenomenonDetails =
                (await _unitOfWork.ExtremePhenomenonDetailRepository.GetWhereAsync(x =>
                    x.ExtremePhenomenonId == extremePhenomenon.Id, cancellationToken)).ToList();
            foreach (var detail in extremePhenomenonDetails)
            {
                var entry = request.Details.FirstOrDefault(x => x.Id == detail.Id);
                if (entry != null)
                {
                    if (!detail.Name.Equals(entry.Name))
                    {
                        detail.Name = entry.Name;
                        isUpdate = true;
                    }
                    if (!detail.Content.Equals(entry.Content))
                    {
                        detail.Content = entry.Content;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.ExtremePhenomenonDetailRepository.Update(detail);
                    }
                }
                else
                {
                    _unitOfWork.ExtremePhenomenonDetailRepository.Delete(detail);
                    isUpdate = true;
                }
            }

            foreach (var detailDto in request.Details.Where(x=>x.Id == Guid.Empty))
            {
                _unitOfWork.ExtremePhenomenonDetailRepository.Add(new ExtremePhenomenonDetail()
                {
                    Id = Guid.NewGuid(),
                    Name = detailDto.Name,
                    Content = detailDto.Content,
                    ExtremePhenomenonId = extremePhenomenon.Id
                });
                isUpdate = true;
            }

            if (isUpdate)
            {
                await _unitOfWork.CommitAsync();
            }

            return extremePhenomenon.Id;
        }

    }
}