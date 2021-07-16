using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon
{
    public class CreateExtremePhenomenonCommandHandler : IRequestHandler<CreateExtremePhenomenonCommand, Guid>
    {
     
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExtremePhenomenonCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Guid> Handle(CreateExtremePhenomenonCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateExtremePhenomenonCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }
            
            var extremePhenomenon = _mapper.Map<ExtremePhenomenon>(request);
            extremePhenomenon.Id = Guid.NewGuid();
            extremePhenomenon.Date = extremePhenomenon.Date.Date;
            _unitOfWork.ExtremePhenomenonRepository.Add(extremePhenomenon);

            var extremePhenomenonDetails = new List<ExtremePhenomenonDetail>();
            foreach (var detail in request.Details)
            {
                extremePhenomenonDetails.Add(new ExtremePhenomenonDetail()
                {
                    Id = Guid.NewGuid(),
                    Content = detail.Content,
                    ExtremePhenomenonId = extremePhenomenon.Id,
                    Name = detail.Name
                });
            }
            _unitOfWork.ExtremePhenomenonDetailRepository.AddRange(extremePhenomenonDetails);
            await _unitOfWork.CommitAsync();

            return  extremePhenomenon.Id;
        }

    }
}