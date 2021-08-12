using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.RemoveActionIcon
{
    public class RemoveActionIconCommandHandler : IRequestHandler<RemoveActionIconCommand>
    {
        private readonly IPostService _postService;
        public RemoveActionIconCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<Unit> Handle(RemoveActionIconCommand request, CancellationToken cancellationToken)
        {
            var validator = new RemoveActionIconCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            await _postService.RemoveActionIconAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}
