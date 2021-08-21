using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentListOfUser
{
    public class GetCommentListOfUserQueryHandle : IRequestHandler<GetCommentListOfUserQuery, GetCommentListOfUserResponse>
    {
        private readonly IPostService _postService;
        public GetCommentListOfUserQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetCommentListOfUserResponse> Handle(GetCommentListOfUserQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetCommentListOfUserAsync(request, cancellationToken);
        }
    }
}
