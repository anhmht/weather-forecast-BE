using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusCommandHander : IRequestHandler<CreateStatusCommand, CreateStatusCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStatusCommandHander(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateStatusCommandResponse> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var createStatusCommandResponse = new CreateStatusCommandResponse();

            var validator = new CreateStatusCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createStatusCommandResponse.Success = false;
                createStatusCommandResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createStatusCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
            
            if (createStatusCommandResponse.Success)
            {
                var status = new Status() { Name = request.Name };
                _unitOfWork.StatusRepository.Add(status);
                await _unitOfWork.CommitAsync();
                createStatusCommandResponse.Status = _mapper.Map<CreateStatusDto>(status);
            }

            return createStatusCommandResponse;
        }
    }
}