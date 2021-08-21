using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval
{
    public class GetCommentDetailForApprovalQueryHandle : IRequestHandler<GetCommentDetailForApprovalQuery, GetCommentDetailForApprovalResponse>
    {
        private readonly IPostService _postService;
        public GetCommentDetailForApprovalQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetCommentDetailForApprovalResponse> Handle(GetCommentDetailForApprovalQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetCommentDetailForApprovalValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            return await _postService.GetCommentDetailForApprovalAsync(request, cancellationToken);
        }
    }
}
