using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList
{
    public class GetCommentListQueryHandle : IRequestHandler<GetCommentListQuery, GetCommentListResponse>
    {
        private readonly IPostService _postService;
        public GetCommentListQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetCommentListResponse> Handle(GetCommentListQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetCommentListAsync(request, cancellationToken);
        }
    }
}
