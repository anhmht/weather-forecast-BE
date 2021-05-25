using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusCommandHander : IRequestHandler<CreateStatusCommand, CreateStatusCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Status> _statusRepository;

        public CreateStatusCommandHander(IMapper mapper, IAsyncRepository<Status> statusRepository)
        {
            _mapper = mapper;
            _statusRepository = statusRepository;
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
                status = await _statusRepository.AddAsync(status);
                createStatusCommandResponse.Status = _mapper.Map<CreateStatusDto>(status);
            }

            return createStatusCommandResponse;
        }
    }
}