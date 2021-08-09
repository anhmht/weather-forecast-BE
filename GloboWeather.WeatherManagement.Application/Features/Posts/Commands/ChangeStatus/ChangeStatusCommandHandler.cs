using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus
{
    public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand>
    {
        private readonly IPostService _postService;
        public ChangeStatusCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var validator = new ChangeStatusCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            await _postService.ChangeStatusAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}
