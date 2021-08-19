using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval
{
    public class GetCommentsForApprovalQueryHandle : IRequestHandler<GetCommentsForApprovalQuery, GetCommentsForApprovalResponse>
    {
        private readonly IPostService _postService;
        public GetCommentsForApprovalQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetCommentsForApprovalResponse> Handle(GetCommentsForApprovalQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetCommentsForApprovalAsync(request, cancellationToken);
        }
    }
}
