using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Persistence.Repositories.Social
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangeStatusAsync(Guid id, int postStatusId, string userName, bool isApproval)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException("Comment", id);
            }


            comment.StatusId = postStatusId;
            if (comment.StatusId == (int)PostStatus.Public)
            {
                comment.PublicDate = DateTime.Now;
            }

            if (isApproval)
            {
                comment.ApprovedByUserName = userName;
            }
            _unitOfWork.CommentRepository.Update(comment);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<List<Comment>> GetListByPostAndUserAsync(List<Guid> postIds, string userName)
        {
            return (await _unitOfWork.CommentRepository.GetWhereAsync(c => postIds.Contains(c.PostId)
                                                                           && c.CreateBy == userName
                                                                           && (c.StatusId == (int) PostStatus.Public
                                                                               || c.StatusId == (int) PostStatus.Private
                                                                               || c.StatusId ==
                                                                               (int) PostStatus.WaitingForApproval)))
                .ToList();
        }

        public async Task<PagedModel<Comment>> GetCommentsForApprovalAsync(GetCommentsForApprovalQuery request, CancellationToken cancellationToken)
        {
            var query = request.StatusIds.Any()
                ? _unitOfWork.CommentRepository.GetWhereQuery(x => request.StatusIds.Contains(x.StatusId))
                : _unitOfWork.CommentRepository.GetWhereQuery(x => x.StatusId == (int)PostStatus.WaitingForApproval);

            return await query
                .OrderBy(x => x.CreateDate).PaginateAsync(request.Page, request.Limit, cancellationToken);
        }
    }
}