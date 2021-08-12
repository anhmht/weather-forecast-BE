using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<bool> ChangeStatusAsync(Guid id, int postStatusId, string userName, bool isApproval);
        Task<PagedModel<Post>> GetPageAsync(GetPostListQuery request,
            CancellationToken cancellationToken);
    }
}