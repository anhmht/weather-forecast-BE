using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval
{
    public class GetPostDetailForApprovalQueryHandle : IRequestHandler<GetPostDetailForApprovalQuery, GetPostDetailForApprovalResponse>
    {
        private readonly IPostService _postService;
        public GetPostDetailForApprovalQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetPostDetailForApprovalResponse> Handle(GetPostDetailForApprovalQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetPostDetailForApprovalValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _postService.GetPostDetailForApprovalAsync(request, cancellationToken);
        }
    }
}
