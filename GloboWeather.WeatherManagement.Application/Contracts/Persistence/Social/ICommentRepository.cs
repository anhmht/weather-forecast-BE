using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
        Task<bool> ChangeStatusAsync(Guid id, int postStatusId, string userName, bool isApproval);
        Task<List<Comment>> GetListByPostAndUserAsync(List<Guid> postIds, string userName);
        Task<PagedModel<Comment>> GetCommentsForApprovalAsync(GetCommentsForApprovalQuery request,
            CancellationToken cancellationToken);
    }
}