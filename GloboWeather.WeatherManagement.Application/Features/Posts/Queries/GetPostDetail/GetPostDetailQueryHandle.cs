using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail
{
    public class GetPostDetailQueryHandle : IRequestHandler<GetPostDetailQuery, GetPostDetailResponse>
    {
        private readonly IPostService _postService;

        public GetPostDetailQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetPostDetailResponse> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetDetailAsync(request, cancellationToken);
        }
    }
}
