﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.UpdateComment;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.RemoveActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.UpdatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Models.Social;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Persistence.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonService _commonService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly AzureStorageConfig _storageConfig;
        private readonly string _loginUserName;

        public PostService(IUnitOfWork unitOfWork, ICommonService commonService, IImageService imageService
            , IMapper mapper, IAuthenticationService authenticationService
            , ILoggedInUserService loggedInUserService
            , IOptions<AzureStorageConfig> azureStorageConfig)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _imageService = imageService;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _storageConfig = azureStorageConfig.Value;
            _loginUserName = loggedInUserService.UserId;
        }

        public async Task<Guid> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                StatusId = (int)PostStatus.WaitingForApproval
            };

            await PopulatePostAsync(request.ImageUrls, request.VideoUrls, post);

            _unitOfWork.PostRepository.Add(post);

            await _unitOfWork.CommitAsync();

            return post.Id;
        }

        public async Task<bool> UpdateAsync(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);

            if (post == null)
            {
                throw new NotFoundException("Post", request.Id);
            }

            if (_loginUserName != post.CreateBy)
            {
                throw new Exception("Only creator can edit the post");
            }

            post.Content = request.Content;
            post.StatusId = (int)PostStatus.WaitingForApproval;

            await PopulatePostAsync(request.ImageUrls, request.VideoUrls, post);

            _unitOfWork.PostRepository.Update(post);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<Guid> CreateCommentAsync(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_loginUserName) &&
                string.IsNullOrEmpty(request.AnonymousUser?.FullName))
            {
                throw new Exception("Must login or provide anonymous info to put comment");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                PostId = request.PostId,
                StatusId = (int)PostStatus.WaitingForApproval
            };

            //If user didn't login -> use anonymous user
            if (string.IsNullOrEmpty(_loginUserName))
            {
                comment.AnonymousUserId = await _unitOfWork.AnonymousUserRepository.Save(request.AnonymousUser);
            }

            await PopulateCommentAsync(request.ImageUrls, request.VideoUrls, comment);

            _unitOfWork.CommentRepository.Add(comment);

            await _unitOfWork.CommitAsync();

            return comment.Id;
        }

        public async Task<bool> UpdateCommentAsync(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);

            if (comment == null)
            {
                throw new NotFoundException("Comment", request.Id);
            }

            if (_loginUserName != comment.CreateBy)
            {
                throw new Exception("Only creator can edit the comment");
            }

            comment.Content = request.Content;
            comment.StatusId = (int)PostStatus.WaitingForApproval;

            await PopulateCommentAsync(request.ImageUrls, request.VideoUrls, comment);

            _unitOfWork.CommentRepository.Update(comment);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> ChangeStatusAsync(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var isApproval = request.IsApproval;

            //If it is not a approval request, check permission of user
            if (!request.IsApproval)
            {
                isApproval = await HasApprovePermission();
            }

            if (request.IsChangePostStatus)
            {
                return await _unitOfWork.PostRepository.ChangeStatusAsync(request.Id, request.PostStatusId,
                    _loginUserName,
                    isApproval);
            }

            return await _unitOfWork.CommentRepository.ChangeStatusAsync(request.Id, request.PostStatusId,
                _loginUserName,
                isApproval);
        }

        public async Task<bool> AddActionIconAsync(AddActionIconCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            return request.IsPost ? await AddPostActionIconAsync(request) : await AddCommentActionIconAsync(request);
        }

        public async Task<bool> RemoveActionIconAsync(RemoveActionIconCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            return request.IsPost ? await RemovePostActionIconAsync(request) : await RemoveCommentActionIconAsync(request);
        }

        public async Task<GetPostListResponse> GetPostListAsync(GetPostListQuery request, CancellationToken cancellationToken)
        {
            if (request.IsUserTimeLine)
            {
                CheckLoginSession();
                request.UserName = _loginUserName;
            }

            var posts = await _unitOfWork.PostRepository.GetPageAsync(request, cancellationToken);

            var postIds = posts.Items.Select(x => x.Id).ToList();

            var publicStatus = (int)PostStatus.Public;
            var comments =
                await _unitOfWork.CommentRepository.GetWhereQuery(
                        x => postIds.Contains(x.PostId) && x.StatusId == publicStatus)
                    .OrderByDescending(x => x.PublicDate)
                    .Take(request.CommentLimit).ToListAsync(cancellationToken: cancellationToken);

            var actionIconList = await (
                from pst in _unitOfWork.PostRepository.GetWhereQuery(x => postIds.Contains(x.Id))
                join cmt in _unitOfWork.CommentRepository.GetWhereQuery(x => x.StatusId == publicStatus)
                    on pst.Id equals cmt.PostId into cmtTemp
                from comment in cmtTemp.DefaultIfEmpty()
                join pact in _unitOfWork.PostActionIconRepository.GetAllQuery()
                    on pst.Id equals pact.PostId into pactTemp
                from postAction in pactTemp.DefaultIfEmpty()
                join cact in _unitOfWork.PostActionIconRepository.GetAllQuery()
                    on comment.Id equals cact.CommentId into cactTemp
                from commentAction in cactTemp.DefaultIfEmpty()
                select new ActionIconDto
                {
                    PostId = postAction.PostId,
                    PostIcon = postAction.IconId,
                    PostActionUserName = postAction.CreateBy,
                    CommentId = commentAction.CommentId,
                    CommentIcon = commentAction.IconId,
                    CommentActionUserName = commentAction.CreateBy
                }).Distinct().ToListAsync(cancellationToken);

            var postShares =
                (await _unitOfWork.SharePostRepository.GetWhereAsync(x => postIds.Contains(x.PostId), cancellationToken)
                ).ToList();

            var anonymousUserIds =
                comments.Where(x => x.AnonymousUserId.HasValue).Select(x => x.AnonymousUserId).ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();

            var listPostVm = _mapper.Map<List<PostVm>>(posts.Items);
            var users = await _authenticationService.GetAllUserAsync();

            foreach (var postItem in listPostVm)
            {
                PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares);
            }

            var response = new GetPostListResponse
            {
                TotalPages = posts.TotalPages,
                CurrentPage = posts.CurrentPage,
                TotalItems = posts.TotalItems,
                Posts = listPostVm
            };

            return response;
        }

        public async Task<GetPostDetailResponse> GetDetailAsync(GetPostDetailQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null)
            {
                throw new NotFoundException("Post", request.Id);
            }

            var publicStatus = (int)PostStatus.Public;
            var comments =
                await _unitOfWork.CommentRepository.GetWhereQuery(
                        x => x.PostId == post.Id && x.StatusId == publicStatus)
                    .OrderBy(x => x.PublicDate)
                    .ToListAsync(cancellationToken: cancellationToken);

            var actionIconList = await (
                from pst in _unitOfWork.PostRepository.GetWhereQuery(x => x.Id == post.Id)
                join cmt in _unitOfWork.CommentRepository.GetWhereQuery(x => x.StatusId == publicStatus)
                    on pst.Id equals cmt.PostId into cmtTemp
                from comment in cmtTemp.DefaultIfEmpty()
                join pact in _unitOfWork.PostActionIconRepository.GetAllQuery()
                    on pst.Id equals pact.PostId into pactTemp
                from postAction in pactTemp.DefaultIfEmpty()
                join cact in _unitOfWork.PostActionIconRepository.GetAllQuery()
                    on comment.Id equals cact.CommentId into cactTemp
                from commentAction in cactTemp.DefaultIfEmpty()
                select new ActionIconDto
                {
                    PostId = postAction.PostId,
                    PostIcon = postAction.IconId,
                    PostActionUserName = postAction.CreateBy,
                    CommentId = commentAction.CommentId,
                    CommentIcon = commentAction.IconId,
                    CommentActionUserName = commentAction.CreateBy
                }).Distinct().ToListAsync(cancellationToken);

            var postShares =
                (await _unitOfWork.SharePostRepository.GetWhereAsync(x => x.PostId == post.Id, cancellationToken)
                ).ToList();

            var anonymousUserIds =
                comments.Where(x => x.AnonymousUserId.HasValue).Select(x => x.AnonymousUserId).ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();

            var postItem = _mapper.Map<GetPostDetailResponse>(post);
            var users = await _authenticationService.GetAllUserAsync();

            PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares);
            
            return postItem;
        }


        #region Private functions
        private void CheckLoginSession()
        {
            if (string.IsNullOrEmpty(_loginUserName))
            {
                throw new Exception("Must login to take this action");
            }
        }

        private async Task<bool> HasApprovePermission()
        {
            var currentUser = await _authenticationService.GetUserInfoByUserNameAsync(_loginUserName);
            return currentUser.Roles.Contains("SuperAdmin"); //Need improve after assigning post moderation to user groups. Current default is SuperAdmin
        }

        private async Task PopulatePostAsync(List<string> requestImageUrls, List<string> requestVideoUrls, Post post)
        {
            if (requestImageUrls?.Any() == true)
            {
                var tempImageUrls = string.Join(Constants.SemiColonStringSeparator, requestImageUrls.OrderBy(x => x));
                if (tempImageUrls != post.ImageUrls)
                {
                    //var postImageUrls = post.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();

                    //Copy new image
                    var imageUrls = await _imageService.CopyFileToStorageContainerAsync(requestImageUrls,
                        post.Id.ToString(),
                        Forder.SocialPost
                        , _storageConfig.SocialPostContainer);
                    post.Content = ReplaceContent.ReplaceImageUrls(post.Content, requestImageUrls, imageUrls);

                    post.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls.OrderBy(x => x));

                    //Delete old image -> do not do this action, because the post has not been approved at this time -> we will create a service auto delete files not use 
                    //if (!string.IsNullOrEmpty(post.ImageUrls))
                    //{
                    //    var deleteImages = new List<string>();
                    //    foreach (var postImageUrl in postImageUrls)
                    //    {
                    //        if (requestImageUrls.Any(x => x == postImageUrl))
                    //            continue;
                    //        deleteImages.Add(postImageUrl);
                    //    }

                    //    if (deleteImages.Any())
                    //    {
                    //        await _imageService.DeleteFileInStorageContainerByNameAsync(post.Id.ToString(),
                    //            deleteImages,
                    //            _storageConfig.SocialPostContainer);
                    //    }
                    //}

                }
            }
            else
            {
                post.ImageUrls = string.Empty;
            }

            if (requestVideoUrls?.Any() == true)
            {
                var tempVideoUrls = string.Join(Constants.SemiColonStringSeparator, requestVideoUrls.OrderBy(x => x));
                if (tempVideoUrls != post.VideoUrls)
                {
                    //var postVideoUrls = post.VideoUrls.Split(Constants.SemiColonStringSeparator).OrderBy(x => x)
                    //    .ToList();

                    //Copy new video
                    var videoUrls = await _imageService.CopyFileToStorageContainerAsync(requestVideoUrls,
                        post.Id.ToString(),
                        Forder.SocialPost
                        , _storageConfig.SocialPostContainer);
                    post.Content = ReplaceContent.ReplaceImageUrls(post.Content, requestVideoUrls, videoUrls);

                    post.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls.OrderBy(x => x));

                    //Delete old video -> do not do this action, because the post has not been approved at this time -> we will create a service auto delete files not use 
                    //if (!string.IsNullOrEmpty(post.VideoUrls))
                    //{
                    //    var deleteVideos = new List<string>();
                    //    foreach (var postVideoUrl in postVideoUrls)
                    //    {
                    //        if (requestVideoUrls.Any(x => x == postVideoUrl))
                    //            continue;
                    //        deleteVideos.Add(postVideoUrl);
                    //    }

                    //    if (deleteVideos.Any())
                    //    {
                    //        await _imageService.DeleteFileInStorageContainerByNameAsync(post.Id.ToString(),
                    //            deleteVideos,
                    //            _storageConfig.SocialPostContainer);
                    //    }
                    //}

                }
            }
            else
            {
                post.VideoUrls = string.Empty;
            }
        }

        private async Task PopulateCommentAsync(List<string> requestImageUrls, List<string> requestVideoUrls, Comment comment)
        {
            if (requestImageUrls?.Any() == true)
            {
                var tempImageUrls = string.Join(Constants.SemiColonStringSeparator, requestImageUrls.OrderBy(x => x));
                if (tempImageUrls != comment.ImageUrls)
                {
                    var imageUrls = await _imageService.CopyFileToStorageContainerAsync(requestImageUrls,
                        comment.PostId.ToString(), Forder.SocialComment
                        , _storageConfig.SocialPostContainer);
                    comment.Content = ReplaceContent.ReplaceImageUrls(comment.Content, requestImageUrls, imageUrls);

                    comment.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls.OrderBy(x => x));
                }
            }
            else
            {
                comment.ImageUrls = string.Empty;
            }

            if (requestVideoUrls?.Any() == true)
            {

                var tempVideoUrls = string.Join(Constants.SemiColonStringSeparator, requestVideoUrls.OrderBy(x => x));
                if (tempVideoUrls != comment.VideoUrls)
                {
                    var videoUrls = await _imageService.CopyFileToStorageContainerAsync(requestVideoUrls,
                        comment.PostId.ToString(), Forder.SocialComment
                        , _storageConfig.SocialPostContainer);
                    comment.Content = ReplaceContent.ReplaceImageUrls(comment.Content, requestVideoUrls, videoUrls);

                    comment.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls.OrderBy(x => x));
                }
            }
            else
            {
                comment.VideoUrls = string.Empty;
            }
        }

        private async Task<bool> AddCommentActionIconAsync(AddActionIconCommand request)
        {
            var actionIconEntry = await _unitOfWork.PostActionIconRepository.FindAsync(x =>
                x.CommentId == request.Id && x.CreateBy == _loginUserName);
            if (actionIconEntry == null)
            {
                actionIconEntry = new PostActionIcon
                {
                    Id = Guid.NewGuid(),
                    CommentId = request.Id,
                    IconId = request.IconId
                };
                _unitOfWork.PostActionIconRepository.Add(actionIconEntry);
            }
            else
            {
                actionIconEntry.IconId = request.IconId;
                _unitOfWork.PostActionIconRepository.Update(actionIconEntry);
            }

            return await _unitOfWork.CommitAsync() > 0;
        }

        private async Task<bool> AddPostActionIconAsync(AddActionIconCommand request)
        {
            var actionIconEntry = await _unitOfWork.PostActionIconRepository.FindAsync(x =>
                x.PostId == request.Id && x.CreateBy == _loginUserName);
            if (actionIconEntry == null)
            {
                actionIconEntry = new PostActionIcon
                {
                    Id = Guid.NewGuid(),
                    PostId = request.Id,
                    IconId = request.IconId
                };
                _unitOfWork.PostActionIconRepository.Add(actionIconEntry);
            }
            else
            {
                actionIconEntry.IconId = request.IconId;
                _unitOfWork.PostActionIconRepository.Update(actionIconEntry);
            }

            return await _unitOfWork.CommitAsync() > 0;
        }

        private async Task<bool> RemovePostActionIconAsync(RemoveActionIconCommand request)
        {
            _unitOfWork.PostActionIconRepository.DeleteWhere(x =>
                x.PostId == request.Id && x.CreateBy == _loginUserName);

            return await _unitOfWork.CommitAsync() > 0;
        }

        private async Task<bool> RemoveCommentActionIconAsync(RemoveActionIconCommand request)
        {
            _unitOfWork.PostActionIconRepository.DeleteWhere(x =>
                x.CommentId == request.Id && x.CreateBy == _loginUserName);

            return await _unitOfWork.CommitAsync() > 0;
        }

        private void PopulatePostActionIcon(List<ActionIconDto> actionIconList, PostVm postItem, List<ApplicationUserDto> users)
        {
            if (actionIconList?.Any() == true)
            {
                foreach (var actionIcon in actionIconList.Where(x => x.PostId == postItem.Id && x.CommentId == null))
                {
                    var actionUser = users.Find(x => x.UserName == actionIcon.PostActionUserName);
                    var postActionIcon = postItem.ActionIcons.Find(x => x.IconId == actionIcon.PostIcon);
                    if (postActionIcon == null)
                    {
                        postActionIcon = new ActionIconVm
                        {
                            IconId = actionIcon.PostIcon.GetInt(),
                            Count = 1
                        };

                        postItem.ActionIcons.Add(postActionIcon);
                    }
                    else
                    {
                        postActionIcon.Count++;
                    }

                    if (actionUser != null)
                    {
                        postActionIcon.FullNames.Add(actionUser.FullName);
                    }

                    if (!postActionIcon.IsCurrentUserChecking)
                        postActionIcon.IsCurrentUserChecking = _loginUserName == actionUser?.UserName;
                }
            }
        }

        private void PopulateCommentActionIcon(List<ActionIconDto> actionIconList, CommentVm commentItem, List<ApplicationUserDto> users)
        {
            if (actionIconList?.Any() == true)
            {
                foreach (var actionIcon in actionIconList.Where(x => x.CommentId == commentItem.Id))
                {
                    var actionUser = users.Find(x => x.UserName == actionIcon.CommentActionUserName);
                    var commentActionIcon = commentItem.ActionIcons.Find(x => x.IconId == actionIcon.CommentIcon);
                    if (commentActionIcon == null)
                    {
                        commentActionIcon = new ActionIconVm
                        {
                            IconId = actionIcon.CommentIcon.GetInt(),
                            Count = 1
                        };

                        commentItem.ActionIcons.Add(commentActionIcon);
                    }
                    else
                    {
                        commentActionIcon.Count++;
                    }

                    if (actionUser != null)
                    {
                        commentActionIcon.FullNames.Add(actionUser.FullName);
                    }

                    if (!commentActionIcon.IsCurrentUserChecking)
                        commentActionIcon.IsCurrentUserChecking = _loginUserName == actionUser?.UserName;
                }
            }
        }

        private void PopulatePostAsync(List<ActionIconDto> actionIconList, PostVm postItem, List<ApplicationUserDto> users, List<Comment> comments, List<AnonymousUser> anonymousUsers,
            List<SharePost> postShares)
        {
            PopulatePostActionIcon(actionIconList, postItem, users);

            var creator = users.Find(x => x.UserName == postItem.CreateBy);
            postItem.ApprovedByFullName = users.Find(x => x.UserName == postItem.ApprovedByUserName)?.FullName;
            postItem.CreatorFullName = creator?.FullName;
            postItem.ListImageUrl = postItem.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
            postItem.ListVideoUrl = postItem.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            postItem.CreatorAvatarUrl = creator?.AvatarUrl;

            var postComments = comments.Where(x => x.PostId == postItem.Id).OrderBy(x => x.CreateDate).ToList();
            postItem.Comments = _mapper.Map<List<CommentVm>>(postComments);
            foreach (var postItemComment in postItem.Comments)
            {
                PopulateCommentActionIcon(actionIconList, postItemComment, users);

                postItemComment.ApprovedByFullName =
                    users.Find(x => x.UserName == postItemComment.ApprovedByUserName)?.FullName;

                if (!string.IsNullOrEmpty(postItemComment.CreateBy))
                {
                    var commentCreator = users.Find(x => x.UserName == postItemComment.CreateBy);
                    postItemComment.CreatorFullName = commentCreator?.FullName;
                    postItemComment.CreatorAvatarUrl = commentCreator?.AvatarUrl;
                }
                else if (postItemComment.AnonymousUserId.HasValue && postItemComment.AnonymousUserId != Guid.Empty)
                {
                    postItemComment.CreatorFullName = anonymousUsers
                        .Find(x => x.Id.Equals(postItemComment.AnonymousUserId))?.FullName;
                }

                postItemComment.ListImageUrl = postItemComment.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
                postItemComment.ListVideoUrl = postItemComment.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            }

            var shares = postShares.Where(x => x.PostId == postItem.Id).ToList();
            postItem.Shares.Count = shares.Count;
            postItem.Shares.FullNames = users.Where(x =>
                    shares.Select(sharePost => sharePost.CreateBy).Contains(x.UserName))
                .Select(x => x.FullName)
                .ToList();
        }

        #endregion
    }
}