using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList
{
    public class GetPostListQueryHandle : IRequestHandler<GetPostListQuery, GetPostListResponse>
    {
        private readonly IPostService _postService;
        public GetPostListQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetPostListResponse> Handle(GetPostListQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetPostListAsync(request, cancellationToken);
        }
    }
}
