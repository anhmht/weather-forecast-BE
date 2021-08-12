using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon
{
    public class AddActionIconCommandHandler : IRequestHandler<AddActionIconCommand>
    {
        private readonly IPostService _postService;
        public AddActionIconCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<Unit> Handle(AddActionIconCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddActionIconCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            await _postService.AddActionIconAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}
