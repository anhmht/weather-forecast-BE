using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
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
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
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
        private readonly IHistoryTrackingService _historyTrackingService;
        private readonly string _clientIpAddress;

        public PostService(IUnitOfWork unitOfWork, ICommonService commonService, IImageService imageService
            , IMapper mapper, IAuthenticationService authenticationService
            , ILoggedInUserService loggedInUserService
            , IOptions<AzureStorageConfig> azureStorageConfig
            , IHistoryTrackingService historyTrackingService)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _imageService = imageService;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _storageConfig = azureStorageConfig.Value;
            _loginUserName = loggedInUserService.UserId;
            _historyTrackingService = historyTrackingService;
            _clientIpAddress = loggedInUserService.IpAddress;
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

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Post), post, null, HistoryTrackingAction.Create, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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

            var originalPost = post.Clone();

            post.Content = request.Content;
            post.StatusId = (int)PostStatus.WaitingForApproval;

            await PopulatePostAsync(request.ImageUrls, request.VideoUrls, post);

            _unitOfWork.PostRepository.Update(post);

            var result = await _unitOfWork.CommitAsync() > 0;

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Post), originalPost, post, HistoryTrackingAction.Update, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return result;
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
                StatusId = (int)PostStatus.WaitingForApproval,
                ParentCommentId = request.ParentCommentId
            };

            //If user didn't login -> use anonymous user
            if (string.IsNullOrEmpty(_loginUserName))
            {
                comment.AnonymousUserId = await _unitOfWork.AnonymousUserRepository.Save(request.AnonymousUser);
            }

            await PopulateCommentAsync(request.ImageUrls, request.VideoUrls, comment);

            _unitOfWork.CommentRepository.Add(comment);

            await _unitOfWork.CommitAsync();

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Comment), comment, null, HistoryTrackingAction.Create, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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

            var originalComment = comment.Clone();

            comment.Content = request.Content;
            comment.StatusId = (int)PostStatus.WaitingForApproval;

            await PopulateCommentAsync(request.ImageUrls, request.VideoUrls, comment);

            _unitOfWork.CommentRepository.Update(comment);
            var result = await _unitOfWork.CommitAsync() > 0;

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Comment), originalComment, comment, HistoryTrackingAction.Update, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return result;
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
                //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _historyTrackingService.SaveAsync(nameof(Post), request, null, HistoryTrackingAction.ChangeStatus, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                return await _unitOfWork.PostRepository.ChangeStatusAsync(request.Id, request.PostStatusId,
                    _loginUserName,
                    isApproval);
            }

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Comment), request, null, HistoryTrackingAction.ChangeStatus, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
                comments.Where(x => x.AnonymousUserId.HasValue).Select(x => x.AnonymousUserId).Distinct().ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();

            var postItem = _mapper.Map<GetPostDetailResponse>(post);
            var users = await _authenticationService.GetAllUserAsync();

            PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares);

            return postItem;
        }

        /// <summary>
        /// Delete old files of the post and the comment if the post/comment had been public
        /// Old files are files that when user update the post/comment, they had changed the images or videos
        /// </summary>
        /// <returns></returns>
        public async Task DeleteTempFile()
        {
            var dicDeleteFile = new Dictionary<Guid, List<string>>();

            var deletePostFiles = await (from d in _unitOfWork.DeleteFileRepository.GetAllQuery()
                join p in _unitOfWork.PostRepository.GetAllQuery()
                    on d.DeleteId equals p.Id
                where d.TableName == nameof(Post) && p.StatusId == (int) PostStatus.Public
                select d).Distinct().ToListAsync();

            foreach (var deletePostFile in deletePostFiles)
            {
                if (dicDeleteFile.ContainsKey(deletePostFile.DeleteId))
                    dicDeleteFile[deletePostFile.DeleteId].Add(deletePostFile.FileUrl);
                else
                {
                    dicDeleteFile[deletePostFile.DeleteId] = new List<string> {deletePostFile.FileUrl};
                }
            }

            var deleteCommentFiles = await (from d in _unitOfWork.DeleteFileRepository.GetAllQuery()
                join p in _unitOfWork.CommentRepository.GetAllQuery()
                    on d.DeleteId equals p.Id
                where d.TableName == nameof(Comment) && p.StatusId == (int)PostStatus.Public
                select d).Distinct().ToListAsync();

            foreach (var deleteCommentFile in deleteCommentFiles)
            {
                if (dicDeleteFile.ContainsKey(deleteCommentFile.DeleteId))
                    dicDeleteFile[deleteCommentFile.DeleteId].Add(deleteCommentFile.FileUrl);
                else
                {
                    dicDeleteFile[deleteCommentFile.DeleteId] = new List<string> { deleteCommentFile.FileUrl };
                }
            }

            foreach (var deleteFile in dicDeleteFile)
            {
                await _imageService.DeleteFileInStorageContainerByNameAsync(deleteFile.Key.ToString(),
                    deleteFile.Value, _storageConfig.SocialPostContainer);
            }

            _unitOfWork.DeleteFileRepository.DeleteRange(deletePostFiles);
            _unitOfWork.DeleteFileRepository.DeleteRange(deleteCommentFiles);

            await _unitOfWork.CommitAsync();
        }

        public async Task<GetCommentListResponse> GetCommentListAsync(GetCommentListQuery request, CancellationToken cancellationToken)
        {
            var publicStatus = (int)PostStatus.Public;
            var pagingModel =
                await _unitOfWork.CommentRepository
                    .GetWhereQuery(x => x.PostId == request.PostId && x.StatusId == publicStatus)
                    .OrderBy(x=>x.PublicDate)
                    .PaginateAsync(request.Page, request.Limit, cancellationToken);

            var comments = _mapper.Map<List<CommentVm>>(pagingModel.Items);

            var commentIds = comments.Select(x => x.Id).ToList();

            var actionIconList = await (
                from cmt in _unitOfWork.CommentRepository.GetWhereQuery(x => commentIds.Contains(x.Id))
                join cact in _unitOfWork.PostActionIconRepository.GetAllQuery()
                    on cmt.Id equals cact.CommentId
                select new ActionIconDto
                {
                    PostId = cmt.Id,
                    CommentId = cact.CommentId,
                    CommentIcon = cact.IconId,
                    CommentActionUserName = cact.CreateBy
                }).Distinct().ToListAsync(cancellationToken);

            var users = await _authenticationService.GetAllUserAsync();

            var anonymousUserIds =
                comments.Where(x => x.AnonymousUserId.HasValue).Select(x => x.AnonymousUserId).Distinct().ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();

            foreach (var commentVm in comments)
            {
                PopulateCommentActionIcon(actionIconList, commentVm, users);
                PopolateCommentVm(comments, users, anonymousUsers, commentVm);
            }

            return new GetCommentListResponse
            {
                CurrentPage = pagingModel.CurrentPage,
                TotalPages = pagingModel.TotalPages,
                Comments = comments,
                TotalItems = pagingModel.TotalItems
            };
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
            var deleteFiles = new List<DeleteFile>();
            var oriPostImageUrls = post.ImageUrls.GetString();
            var oriPostVideoUrls = post.VideoUrls.GetString();

            if (requestImageUrls?.Any() == true)
            {
                var tempImageUrls = string.Join(Constants.SemiColonStringSeparator, requestImageUrls.OrderBy(x => x));
                if (tempImageUrls != post.ImageUrls)
                {
                    //Copy new image
                    var imageUrls = await _imageService.CopyFileToStorageContainerAsync(requestImageUrls,
                        post.Id.ToString(),
                        Forder.SocialPost
                        , _storageConfig.SocialPostContainer);
                    post.Content = ReplaceContent.ReplaceImageUrls(post.Content, requestImageUrls, imageUrls);

                    post.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls.OrderBy(x => x));
                }
            }
            else
            {
                post.ImageUrls = string.Empty;
            }

            //Add to temp table to wait for deleting
            if (oriPostImageUrls != post.ImageUrls)
            {
                var tempOriPostImageUrls = oriPostImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
                var newPostImageUrls = post.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
                deleteFiles.AddRange(from imageUrl in tempOriPostImageUrls
                                     where newPostImageUrls.All(x => x != imageUrl)
                                     select new DeleteFile
                                     {
                                         Id = Guid.NewGuid(),
                                         ContainerName = _storageConfig.SocialPostContainer,
                                         DeleteId = post.Id,
                                         FileUrl = imageUrl,
                                         TableName = nameof(Post)
                                     });
            }

            if (requestVideoUrls?.Any() == true)
            {
                var tempVideoUrls = string.Join(Constants.SemiColonStringSeparator, requestVideoUrls.OrderBy(x => x));
                if (tempVideoUrls != post.VideoUrls)
                {
                    //Copy new video
                    var videoUrls = await _imageService.CopyFileToStorageContainerAsync(requestVideoUrls,
                        post.Id.ToString(),
                        Forder.SocialPost
                        , _storageConfig.SocialPostContainer);
                    post.Content = ReplaceContent.ReplaceImageUrls(post.Content, requestVideoUrls, videoUrls);

                    post.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls.OrderBy(x => x));
                }
            }
            else
            {
                post.VideoUrls = string.Empty;
            }

            //Add to temp table to wait for deleting
            if (oriPostVideoUrls != post.VideoUrls)
            {
                var tempOriPostVideoUrls = oriPostVideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
                var newPostVideoUrls = post.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
                deleteFiles.AddRange(from videoUrl in tempOriPostVideoUrls
                                     where newPostVideoUrls.All(x => x != videoUrl)
                                     select new DeleteFile
                                     {
                                         Id = Guid.NewGuid(),
                                         ContainerName = _storageConfig.SocialPostContainer,
                                         DeleteId = post.Id,
                                         FileUrl = videoUrl,
                                         TableName = nameof(Post)
                                     });
            }

            if (deleteFiles.Any())
            {
                _unitOfWork.DeleteFileRepository.AddRange(deleteFiles);
            }
        }

        private async Task PopulateCommentAsync(List<string> requestImageUrls, List<string> requestVideoUrls, Comment comment)
        {
            var deleteFiles = new List<DeleteFile>();
            var oriPostImageUrls = comment.ImageUrls.GetString();
            var oriPostVideoUrls = comment.VideoUrls.GetString();

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

            //Add to temp table to wait for deleting
            if (oriPostImageUrls != comment.ImageUrls)
            {
                var tempOriPostImageUrls = oriPostImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
                var newPostImageUrls = comment.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
                deleteFiles.AddRange(from imageUrl in tempOriPostImageUrls
                                     where newPostImageUrls.All(x => x != imageUrl)
                                     select new DeleteFile
                                     {
                                         Id = Guid.NewGuid(),
                                         ContainerName = _storageConfig.SocialPostContainer,
                                         DeleteId = comment.Id,
                                         FileUrl = imageUrl,
                                         TableName = nameof(Comment)
                                     });
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

            //Add to temp table to wait for deleting
            if (oriPostVideoUrls != comment.VideoUrls)
            {
                var tempOriPostVideoUrls = oriPostVideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
                var newPostVideoUrls = comment.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
                deleteFiles.AddRange(from videoUrl in tempOriPostVideoUrls
                                     where newPostVideoUrls.All(x => x != videoUrl)
                                     select new DeleteFile
                                     {
                                         Id = Guid.NewGuid(),
                                         ContainerName = _storageConfig.SocialPostContainer,
                                         DeleteId = comment.Id,
                                         FileUrl = videoUrl,
                                         TableName = nameof(Comment)
                                     });
            }

            if (deleteFiles.Any())
            {
                _unitOfWork.DeleteFileRepository.AddRange(deleteFiles);
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

        private void PopulatePostAsync(List<ActionIconDto> actionIconList, PostVm postItem,
            List<ApplicationUserDto> users, List<Comment> comments, List<AnonymousUser> anonymousUsers,
            List<SharePost> postShares)
        {
            PopulatePostActionIcon(actionIconList, postItem, users);

            var creator = users.Find(x => x.UserName == postItem.CreateBy);
            postItem.ApprovedByFullName = users.Find(x => x.UserName == postItem.ApprovedByUserName)?.FullName;
            postItem.CreatorFullName = creator?.FullName;
            postItem.CreatorShortName = creator?.ShortName;
            postItem.ListImageUrl = postItem.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
            postItem.ListVideoUrl = postItem.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            postItem.CreatorAvatarUrl = creator?.AvatarUrl;

            var postComments = comments.Where(x => x.PostId == postItem.Id).OrderBy(x => x.CreateDate).ToList();
            postItem.Comments = _mapper.Map<List<CommentVm>>(postComments);
            postItem.NumberOfComment = postItem.Comments.Count;
            foreach (var postItemComment in postItem.Comments)
            {
                PopulateCommentActionIcon(actionIconList, postItemComment, users);

                PopolateCommentVm(postItem.Comments, users, anonymousUsers, postItemComment);
            }

            var shares = postShares.Where(x => x.PostId == postItem.Id).ToList();
            postItem.Shares.Count = shares.Count;
            postItem.Shares.FullNames = users.Where(x =>
                    shares.Select(sharePost => sharePost.CreateBy).Contains(x.UserName))
                .Select(x => x.FullName)
                .ToList();
        }

        private static void PopolateCommentVm(List<CommentVm> comments, List<ApplicationUserDto> users,
            List<AnonymousUser> anonymousUsers, CommentVm commentVm)
        {
            commentVm.ApprovedByFullName =
                users.Find(x => x.UserName == commentVm.ApprovedByUserName)?.FullName;

            if (!string.IsNullOrEmpty(commentVm.CreateBy))
            {
                var commentCreator = users.Find(x => x.UserName == commentVm.CreateBy);
                commentVm.CreatorFullName = commentCreator?.FullName;
                commentVm.CreatorShortName = commentCreator?.ShortName;
                commentVm.CreatorAvatarUrl = commentCreator?.AvatarUrl;
            }
            else if (commentVm.AnonymousUserId.HasValue && commentVm.AnonymousUserId != Guid.Empty)
            {
                var anonymousUser = anonymousUsers
                    .Find(x => x.Id.Equals(commentVm.AnonymousUserId));
                if (anonymousUser != null)
                {
                    commentVm.CreatorFullName = anonymousUser.FullName;
                    commentVm.CreatorShortName =
                        string.Join("",
                            anonymousUser.FullName.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.First()));
                }
            }

            commentVm.ListImageUrl = commentVm.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
            commentVm.ListVideoUrl = commentVm.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            commentVm.NumberOfSubComment = comments.Count(x => x.ParentCommentId == commentVm.Id);
        }

        #endregion
    }
}
