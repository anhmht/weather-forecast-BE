using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.UpdateComment;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.RemoveActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.UpdatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentListOfUser;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostsForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetSubComments;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(UpdatePostCommand request, CancellationToken cancellationToken);
        Task<Guid> CreateCommentAsync(CreateCommentCommand request, CancellationToken cancellationToken);
        Task<bool> UpdateCommentAsync(UpdateCommentCommand request, CancellationToken cancellationToken);
        Task<bool> ChangeStatusAsync(ChangeStatusCommand request, CancellationToken cancellationToken);
        Task<bool> AddActionIconAsync(AddActionIconCommand request, CancellationToken cancellationToken);
        Task<bool> RemoveActionIconAsync(RemoveActionIconCommand request, CancellationToken cancellationToken);
        Task<GetPostListResponse> GetPostListAsync(GetPostListQuery request,
            CancellationToken cancellationToken);
        Task<GetPostDetailResponse> GetDetailAsync(GetPostDetailQuery request,
            CancellationToken cancellationToken);

        Task DeleteTempFile();
        Task<GetCommentListResponse> GetCommentListAsync(GetCommentListQuery request,
            CancellationToken cancellationToken);
        Task<GetCommentListOfUserResponse> GetCommentListOfUserAsync(GetCommentListOfUserQuery request,
            CancellationToken cancellationToken);
        Task<GetSubCommentsResponse> GetSubCommentsAsync(GetSubCommentsQuery request,
            CancellationToken cancellationToken);
        Task<GetPostsForApprovalResponse> GetPostsForApprovalAsync(GetPostsForApprovalQuery request,
            CancellationToken cancellationToken);
        Task<GetCommentsForApprovalResponse> GetCommentsForApprovalAsync(GetCommentsForApprovalQuery request,
            CancellationToken cancellationToken);

        Task<GetPostDetailForApprovalResponse> GetPostDetailForApprovalAsync(GetPostDetailForApprovalQuery request,
            CancellationToken cancellationToken);
        Task<GetCommentDetailForApprovalResponse> GetCommentDetailForApprovalAsync(GetCommentDetailForApprovalQuery request,
            CancellationToken cancellationToken);
    }
}
