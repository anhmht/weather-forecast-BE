using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetSubComments
{
    public class GetSubCommentsQueryHandle : IRequestHandler<GetSubCommentsQuery, GetSubCommentsResponse>
    {
        private readonly IPostService _postService;
        public GetSubCommentsQueryHandle(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<GetSubCommentsResponse> Handle(GetSubCommentsQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetSubCommentsAsync(request, cancellationToken);
        }
    }
}
