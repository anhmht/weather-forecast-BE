using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
    {
        private readonly IPostService _postService;
        public CreateCommentCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCommentCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _postService.CreateCommentAsync(request, cancellationToken);
        }
    }
}
