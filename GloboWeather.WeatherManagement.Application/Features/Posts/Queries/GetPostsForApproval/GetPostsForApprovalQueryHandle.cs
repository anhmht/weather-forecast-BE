using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostsForApproval
{
    public class GetPostsForApprovalQueryHandle : IRequestHandler<GetPostsForApprovalQuery, GetPostsForApprovalResponse>
    {
        private readonly IPostService _postService;
        public GetPostsForApprovalQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetPostsForApprovalResponse> Handle(GetPostsForApprovalQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetPostsForApprovalAsync(request, cancellationToken);
        }
    }
}
