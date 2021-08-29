using System;
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
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentListOfUser;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostsForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetSubComments;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Social;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Application.SignalRClient;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
        private readonly ISignalRClient _signalRClient;
        private bool _isOpenningSignalRConnect;

        public PostService(IUnitOfWork unitOfWork, ICommonService commonService, IImageService imageService
            , IMapper mapper, IAuthenticationService authenticationService
            , ILoggedInUserService loggedInUserService
            , IOptions<AzureStorageConfig> azureStorageConfig
            , IHistoryTrackingService historyTrackingService
            , ISignalRClient signalRClient)
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
            _signalRClient = signalRClient;
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

            //Push notification to all SuperAdmin
            var receivers =
                (await _authenticationService.GetUsersInRoleAsync(ApplicationUserRole.SuperAdmin)).Select(x =>
                    x.UserName);
            await PushNotification(receivers, post.Id, null, NotificationAction.CreatePost);

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

            //Push notification to all SuperAdmin
            var receivers =
                (await _authenticationService.GetUsersInRoleAsync(ApplicationUserRole.SuperAdmin)).Select(x =>
                    x.UserName);
            await PushNotification(receivers, post.Id, null, NotificationAction.EditPost);

            await _unitOfWork.CommitAsync();

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

            //Push notification to all SuperAdmin
            var receivers =
                (await _authenticationService.GetUsersInRoleAsync(ApplicationUserRole.SuperAdmin)).Select(x =>
                    x.UserName);
            await PushNotification(receivers, null, comment.Id, NotificationAction.CreateComment, comment.AnonymousUserId);

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

            //Push notification to all SuperAdmin
            var receivers =
                (await _authenticationService.GetUsersInRoleAsync(ApplicationUserRole.SuperAdmin)).Select(x =>
                    x.UserName);
            await PushNotification(receivers, null, comment.Id, NotificationAction.EditComment);

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

                var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
                if (post == null)
                    return false;
                if (_loginUserName != post.CreateBy && request.PostStatusId != (int)PostStatus.Private)
                {
                    //Push notification
                    await PushNotification(post.CreateBy, post.Id, null,
                        NotificationAction.ChangePostStatus);
                }

                return await _unitOfWork.PostRepository.ChangeStatusAsync(request.Id, request.PostStatusId,
                    _loginUserName,
                    isApproval);
            }

            //Save history tracking
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _historyTrackingService.SaveAsync(nameof(Comment), request, null, HistoryTrackingAction.ChangeStatus, _clientIpAddress);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);
            if (comment == null)
                return false;
            if (_loginUserName != comment.CreateBy && request.PostStatusId != (int)PostStatus.Private)
            {
                var receivers = new List<string> { comment.CreateBy };
                var post = await _unitOfWork.PostRepository.GetByIdAsync(comment.PostId);
                if (!string.IsNullOrEmpty(post?.CreateBy))
                {
                    receivers.Add(post.CreateBy);
                }

                if (comment.ParentCommentId.HasValue && !comment.ParentCommentId.Equals(Guid.Empty))
                {
                    var parentComment = await _unitOfWork.CommentRepository.GetByIdAsync(comment.ParentCommentId.Value);
                    if (!string.IsNullOrEmpty(parentComment?.CreateBy))
                        receivers.Add(parentComment.CreateBy);
                }

                //Push notification
                await PushNotification(receivers, comment.Id, null,
                    NotificationAction.ChangeCommentStatus);
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
            var commentQuery = _unitOfWork.CommentRepository.GetWhereQuery(
                    x => postIds.Contains(x.PostId) && x.StatusId == publicStatus);

            var comments = await commentQuery.Where(x => !x.ParentCommentId.HasValue).Take(request.CommentLimit)
                .OrderBy(x => x.PublicDate)
                .ToListAsync(cancellationToken: cancellationToken);

            var countCommentOfPost = await (from c in commentQuery
                                            group c by c.PostId
                into grp
                                            select new { PostId = grp.Key, NumberOfComment = grp.Count() }).ToDictionaryAsync(x => x.PostId,
                y => y.NumberOfComment, cancellationToken: cancellationToken);

            var countSubComment = await (from c in commentQuery.Where(x => x.ParentCommentId.HasValue)
                                         group c by c.ParentCommentId
                    into grp
                                         select new { CommentId = grp.Key.Value, NumberOfSubComment = grp.Count() })
                .ToDictionaryAsync(x => x.CommentId, y => y.NumberOfSubComment, cancellationToken: cancellationToken);

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
                PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares, countCommentOfPost, countSubComment);
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

            var commentQuery = _unitOfWork.CommentRepository.GetWhereQuery(
                    x => x.PostId == post.Id && x.StatusId == publicStatus)
                .OrderBy(x => x.PublicDate);

            var comments = await commentQuery.Where(x => !x.ParentCommentId.HasValue)
                .ToListAsync(cancellationToken: cancellationToken);

            var countCommentOfPost = await (from c in commentQuery
                                            group c by c.PostId
                into grp
                                            select new { PostId = grp.Key, NumberOfComment = grp.Count() }).ToDictionaryAsync(x => x.PostId,
                y => y.NumberOfComment, cancellationToken: cancellationToken);

            var countSubComment = await (from c in commentQuery.Where(x => x.ParentCommentId.HasValue)
                                         group c by c.ParentCommentId
                    into grp
                                         select new { CommentId = grp.Key.Value, NumberOfSubComment = grp.Count() })
                .ToDictionaryAsync(x => x.CommentId, y => y.NumberOfSubComment, cancellationToken: cancellationToken);

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

            PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares, countCommentOfPost, countSubComment);

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
                                         where d.TableName == nameof(Post) && p.StatusId == (int)PostStatus.Public
                                         select d).Distinct().ToListAsync();

            foreach (var deletePostFile in deletePostFiles)
            {
                if (dicDeleteFile.ContainsKey(deletePostFile.DeleteId))
                    dicDeleteFile[deletePostFile.DeleteId].Add(deletePostFile.FileUrl);
                else
                {
                    dicDeleteFile[deletePostFile.DeleteId] = new List<string> { deletePostFile.FileUrl };
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
            var commentQuery = _unitOfWork.CommentRepository
                .GetWhereQuery(x => x.PostId == request.PostId && x.StatusId == publicStatus);
            var pagingModel = await commentQuery.Where(x => !x.ParentCommentId.HasValue)
                .OrderBy(x => x.PublicDate)
                .PaginateAsync(request.Page, request.Limit, cancellationToken);

            var comments = _mapper.Map<List<CommentVm>>(pagingModel.Items);

            var countSubComment = await (from c in commentQuery.Where(x => x.ParentCommentId.HasValue)
                                         group c by c.ParentCommentId
                    into grp
                                         select new { CommentId = grp.Key.Value, NumberOfSubComment = grp.Count() })
                .ToDictionaryAsync(x => x.CommentId, y => y.NumberOfSubComment, cancellationToken: cancellationToken);

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
                PopulateCommentVm(users, anonymousUsers, commentVm, countSubComment);
            }

            return new GetCommentListResponse
            {
                CurrentPage = pagingModel.CurrentPage,
                TotalPages = pagingModel.TotalPages,
                Comments = comments,
                TotalItems = pagingModel.TotalItems
            };
        }

        public async Task<GetCommentListOfUserResponse> GetCommentListOfUserAsync(GetCommentListOfUserQuery request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var posts = await _unitOfWork.PostRepository.GetPostByUserCommentedAsync(request, _loginUserName, cancellationToken);

            var postIds = posts.Items.Select(x => x.Id).ToList();

            var commentQuery = _unitOfWork.CommentRepository.GetWhereQuery(c => postIds.Contains(c.PostId)
                                                                                && c.CreateBy == _loginUserName
                                                                                && (c.StatusId ==
                                                                                    (int)PostStatus.Public
                                                                                    || c.StatusId ==
                                                                                    (int)PostStatus.Private
                                                                                    || c.StatusId ==
                                                                                    (int)PostStatus
                                                                                        .WaitingForApproval));
            var comments = await commentQuery.OrderBy(x => x.PublicDate)
                .ToListAsync(cancellationToken: cancellationToken);

            var countCommentOfPost = await (from c in commentQuery
                                            group c by c.PostId
                into grp
                                            select new { PostId = grp.Key, NumberOfComment = grp.Count() }).ToDictionaryAsync(x => x.PostId,
                y => y.NumberOfComment, cancellationToken: cancellationToken);

            var countSubComment = await (from c in commentQuery.Where(x => x.ParentCommentId.HasValue)
                                         group c by c.ParentCommentId
                    into grp
                                         select new { CommentId = grp.Key.Value, NumberOfSubComment = grp.Count() })
                .ToDictionaryAsync(x => x.CommentId, y => y.NumberOfSubComment, cancellationToken: cancellationToken);

            var actionIconList = await (
                from pst in _unitOfWork.PostRepository.GetWhereQuery(x => postIds.Contains(x.Id))
                join cmt in _unitOfWork.CommentRepository.GetWhereQuery(x => x.CreateBy == _loginUserName)
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
                PopulatePostAsync(actionIconList, postItem, users, comments, anonymousUsers, postShares, countCommentOfPost, countSubComment);
            }

            var response = new GetCommentListOfUserResponse
            {
                TotalPages = posts.TotalPages,
                CurrentPage = posts.CurrentPage,
                TotalItems = posts.TotalItems,
                Items = listPostVm
            };

            return response;
        }

        public async Task<GetSubCommentsResponse> GetSubCommentsAsync(GetSubCommentsQuery request, CancellationToken cancellationToken)
        {
            CheckLoginSession();
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment", request.CommentId);
            }

            var commentQuery = _unitOfWork.CommentRepository.GetWhereQuery(x =>
                x.PostId == comment.PostId && x.StatusId == (int)PostStatus.Public);

            var commentsPaging = await commentQuery.Where(x => x.ParentCommentId == request.CommentId)
                .OrderBy(x => x.PublicDate).PaginateAsync(request.Page, request.Limit, cancellationToken);

            var comments = _mapper.Map<List<CommentVm>>(commentsPaging.Items);
            var commentIds = comments.Select(x => x.Id).ToList();

            var countSubComment = await (from c in commentQuery.Where(x =>
                        x.ParentCommentId.HasValue && commentIds.Contains(x.ParentCommentId.Value))
                                         group c by c.ParentCommentId
                    into grp
                                         select new { CommentId = grp.Key.Value, NumberOfSubComment = grp.Count() })
                .ToDictionaryAsync(x => x.CommentId, y => y.NumberOfSubComment, cancellationToken: cancellationToken);

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


            foreach (var postItemComment in comments)
            {
                PopulateCommentActionIcon(actionIconList, postItemComment, users);

                PopulateCommentVm(users, anonymousUsers, postItemComment, countSubComment);
            }

            return new GetSubCommentsResponse
            {
                Items = comments,
                CurrentPage = commentsPaging.CurrentPage,
                TotalPages = commentsPaging.TotalPages,
                TotalItems = commentsPaging.TotalItems
            };
        }

        public async Task<GetPostsForApprovalResponse> GetPostsForApprovalAsync(GetPostsForApprovalQuery request, CancellationToken cancellationToken)
        {
            var postPaging = await _unitOfWork.PostRepository.GetPostsForApprovalAsync(request, cancellationToken);

            var posts = _mapper.Map<List<PostForApprovalVm>>(postPaging.Items);

            var users = await _authenticationService.GetAllUserAsync();

            var postStatuses = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.PostStatus);

            foreach (var postForApprovalVm in posts)
            {
                var user = users.Find(x => x.UserName == postForApprovalVm.CreateBy);
                var statusName = postStatuses.Find(x => x.ValueId == postForApprovalVm.StatusId)?.Description;
                var post = postPaging.Items.FirstOrDefault(x => x.Id == postForApprovalVm.Id);
                postForApprovalVm.CreatorAvatarUrl = user?.AvatarUrl;
                postForApprovalVm.CreatorFullName = user?.FullName;
                postForApprovalVm.CreatorShortName = user?.ShortName;
                postForApprovalVm.StatusName = statusName;
                postForApprovalVm.HasMedia =
                    !(string.IsNullOrEmpty(post?.ImageUrls) && string.IsNullOrEmpty(post?.VideoUrls));
            }

            return new GetPostsForApprovalResponse
            {
                Items = posts,
                CurrentPage = postPaging.CurrentPage,
                TotalPages = postPaging.TotalPages,
                TotalItems = postPaging.TotalItems
            };
        }

        public async Task<GetCommentsForApprovalResponse> GetCommentsForApprovalAsync(GetCommentsForApprovalQuery request, CancellationToken cancellationToken)
        {
            var pagingList = await _unitOfWork.CommentRepository.GetCommentsForApprovalAsync(request, cancellationToken);

            var comments = _mapper.Map<List<CommentForApprovalVm>>(pagingList.Items);

            var users = await _authenticationService.GetAllUserAsync();

            var postStatuses = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.PostStatus);

            var anonymousUserIds =
                comments.Where(x => x.AnonymousUserId.HasValue).Select(x => x.AnonymousUserId).Distinct().ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();

            foreach (var commentForApprovalVm in comments)
            {
                var user = users.Find(x => x.UserName == commentForApprovalVm.CreateBy);
                var statusName = postStatuses.Find(x => x.ValueId == commentForApprovalVm.StatusId)?.Description;
                var comment = pagingList.Items.FirstOrDefault(x => x.Id == commentForApprovalVm.Id);
                if (user != null)
                {
                    commentForApprovalVm.CreatorAvatarUrl = user.AvatarUrl;
                    commentForApprovalVm.CreatorFullName = user.FullName;
                    commentForApprovalVm.CreatorShortName = user.ShortName;
                }
                else
                {
                    var anonymousUser = anonymousUsers.Find(x => x.Id == commentForApprovalVm.AnonymousUserId);
                    commentForApprovalVm.CreatorFullName = anonymousUser.FullName;
                    commentForApprovalVm.CreatorShortName =
                        string.Join("",
                            anonymousUser.FullName.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.First()));
                }

                commentForApprovalVm.StatusName = statusName;
                commentForApprovalVm.HasMedia =
                    !(string.IsNullOrEmpty(comment?.ImageUrls) && string.IsNullOrEmpty(comment?.VideoUrls));
            }

            return new GetCommentsForApprovalResponse
            {
                Items = comments,
                CurrentPage = pagingList.CurrentPage,
                TotalPages = pagingList.TotalPages,
                TotalItems = pagingList.TotalItems
            };
        }

        public async Task<GetPostDetailForApprovalResponse> GetPostDetailForApprovalAsync(GetPostDetailForApprovalQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            var response = _mapper.Map<GetPostDetailForApprovalResponse>(post);

            var user = (await _authenticationService.GetAllUserAsync()).Find(x => x.UserName == post.CreateBy);

            response.CreatorAvatarUrl = user?.AvatarUrl;
            response.CreatorFullName = user?.FullName;
            response.CreatorShortName = user?.ShortName;

            if (!string.IsNullOrEmpty(post.ImageUrls))
            {
                response.ListImageUrl = post.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
            }
            if (!string.IsNullOrEmpty(post.VideoUrls))
            {
                response.ListVideoUrl = post.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            }

            return response;
        }

        public async Task<GetCommentDetailForApprovalResponse> GetCommentDetailForApprovalAsync(GetCommentDetailForApprovalQuery request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);
            var response = _mapper.Map<GetCommentDetailForApprovalResponse>(comment);

            var user = (await _authenticationService.GetAllUserAsync()).Find(x => x.UserName == comment.CreateBy);

            if (user != null)
            {
                response.CreatorAvatarUrl = user.AvatarUrl;
                response.CreatorFullName = user.FullName;
                response.CreatorShortName = user.ShortName;
            }
            else if (comment.AnonymousUserId != null)
            {
                var anonymousUser = await _unitOfWork.AnonymousUserRepository.GetByIdAsync(comment.AnonymousUserId.Value);
                response.CreatorFullName = anonymousUser.FullName;
                response.CreatorShortName =
                    string.Join("",
                        anonymousUser.FullName.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.First()));
            }

            if (!string.IsNullOrEmpty(comment.ImageUrls))
            {
                response.ListImageUrl = comment.ImageUrls.Split(Constants.SemiColonStringSeparator).ToList();
            }
            if (!string.IsNullOrEmpty(comment.VideoUrls))
            {
                response.ListVideoUrl = comment.VideoUrls.Split(Constants.SemiColonStringSeparator).ToList();
            }

            return response;
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
                x.CommentId == request.Id && x.CreateBy == _loginUserName && x.IconId == request.IconId);
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

            //Push notification
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);
            if (comment != null && _loginUserName != comment.CreateBy)
            {
                var actionIcons = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionIcon);

                await PushNotification(comment.CreateBy, null, actionIconEntry.Id,
                    actionIcons.Find(x => x.ValueId == request.IconId).ValueText);
            }

            return await _unitOfWork.CommitAsync() > 0;
        }

        private async Task<bool> AddPostActionIconAsync(AddActionIconCommand request)
        {
            var actionIconEntry = await _unitOfWork.PostActionIconRepository.FindAsync(x =>
                x.PostId == request.Id && x.CreateBy == _loginUserName && x.IconId == request.IconId);
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

            //Push notification
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post != null && _loginUserName != post.CreateBy)
            {
                var actionIcons = await _commonService.GetCommonLookupByNameSpaceAsync(LookupNameSpace.ActionIcon);

                await PushNotification(post.CreateBy, actionIconEntry.Id, null,
                    actionIcons.Find(x => x.ValueId == request.IconId).ValueText);
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
            List<SharePost> postShares, Dictionary<Guid, int> countCommentOfPost, Dictionary<Guid, int> countSubComment)
        {
            PopulatePostActionIcon(actionIconList, postItem, users);

            var creator = users.Find(x => x.UserName == postItem.CreateBy);
            postItem.ApprovedByFullName = users.Find(x => x.UserName == postItem.ApprovedByUserName)?.FullName;
            postItem.CreatorFullName = creator?.FullName;
            postItem.CreatorShortName = creator?.ShortName;
            postItem.ListImageUrl = postItem.ImageUrls.Split(Constants.SemiColonStringSeparator)
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            postItem.ListVideoUrl = postItem.VideoUrls.Split(Constants.SemiColonStringSeparator)
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            postItem.CreatorAvatarUrl = creator?.AvatarUrl;

            var postComments = comments.Where(x => x.PostId == postItem.Id).ToList();
            postItem.Comments = _mapper.Map<List<CommentVm>>(postComments);
            postItem.NumberOfComment =
                countCommentOfPost.TryGetValue(postItem.Id, out var numberOfComment) ? numberOfComment : 0;
            foreach (var postItemComment in postItem.Comments)
            {
                PopulateCommentActionIcon(actionIconList, postItemComment, users);

                PopulateCommentVm(users, anonymousUsers, postItemComment, countSubComment);
            }

            var shares = postShares.Where(x => x.PostId == postItem.Id).ToList();
            postItem.Shares.Count = shares.Count;
            postItem.Shares.FullNames = users.Where(x =>
                    shares.Select(sharePost => sharePost.CreateBy).Contains(x.UserName))
                .Select(x => x.FullName)
                .ToList();
        }

        private static void PopulateCommentVm(List<ApplicationUserDto> users,
            List<AnonymousUser> anonymousUsers, CommentVm commentVm, Dictionary<Guid, int> countSubComment)
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

            commentVm.ListImageUrl = commentVm.ImageUrls.Split(Constants.SemiColonStringSeparator)
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            commentVm.ListVideoUrl = commentVm.VideoUrls.Split(Constants.SemiColonStringSeparator)
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            commentVm.NumberOfSubComment = countSubComment.TryGetValue(commentVm.Id, out var numberOfSubComment) ? numberOfSubComment : 0;
        }

        private async Task PushNotification(string receiver, Guid? postId, Guid? commentId, string action, Guid? anonymousUserId = null,
            string description = "")
        {
            await PushNotification(new List<string> { receiver }, postId, commentId, action, anonymousUserId, description);
        }
        private async Task PushNotification(IEnumerable<string> receivers, Guid? postId, Guid? commentId, string action
            , Guid? anonymousUserId = null, string description = "")
        {
            //if (!_isOpenningSignalRConnect)
            //{
            //    await _signalRClient.StartConnectAsync(_loginUserName);
            //    _isOpenningSignalRConnect = true;
            //}

            foreach (var receiver in receivers)
            {
                var notification = new SocialNotification
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    Action = action,
                    Receiver = receiver,
                    CommentId = commentId,
                    Description = description,
                    CreateBy = _loginUserName,
                    AnonymousUserId = anonymousUserId
                };

                _unitOfWork.SocialNotificationRepository.Add(notification);

                //try
                //{
                //    await _signalRClient.SendMessageToUser(receiver, JsonConvert.SerializeObject(notification));
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //    //throw new Exception($"Need write log when push notification error{Environment.NewLine}{e}");
                //}

            }
        }

        #endregion
    }
}
