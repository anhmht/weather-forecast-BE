using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
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
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly AzureStorageConfig _storageConfig;

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
            _loggedInUserService = loggedInUserService;
            _storageConfig = azureStorageConfig.Value;
        }

        public async Task<Guid> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CheckLoginSession();

            var post = new Post
            {
                Id = Guid.NewGuid(), Content = request.Content, StatusId = (int) PostStatus.WaitingForApproval
            };

            if (request.ImageUrls?.Any() == true)
            {
                var imageUrls = await _imageService.CopyFileToStorageContainerAsync(request.ImageUrls, post.Id.ToString(), Forder.SocialPost
                    , _storageConfig.SocialPostContainer);
                post.Content = ReplaceContent.ReplaceImageUrls(post.Content, request.ImageUrls, imageUrls);

                post.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls);
            }

            if (request.VideoUrls?.Any() == true)
            {
                var videoUrls = await _imageService.CopyFileToStorageContainerAsync(request.VideoUrls, post.Id.ToString(), Forder.SocialPost
                    , _storageConfig.SocialPostContainer);
                post.Content = ReplaceContent.ReplaceImageUrls(post.Content, request.VideoUrls, videoUrls);

                post.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls);
            }

            _unitOfWork.PostRepository.Add(post);

            await _unitOfWork.CommitAsync();

            return post.Id;
        }

        public async Task<Guid> CreateCommentAsync(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_loggedInUserService.UserId) &&
                string.IsNullOrEmpty(request.AnonymousUser?.FullName))
            {
                throw new Exception("Must login or provide anonymous info to put comment");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(), 
                Content = request.Content, 
                PostId = request.PostId,
                StatusId = (int) PostStatus.WaitingForApproval
            };

            //If user didn't login -> use anonymous user
            if (string.IsNullOrEmpty(_loggedInUserService.UserId))
            {
                comment.AnonymousUserId = await _unitOfWork.AnonymousUserRepository.Save(request.AnonymousUser);
            }

            if (request.ImageUrls?.Any() == true)
            {
                var imageUrls = await _imageService.CopyFileToStorageContainerAsync(request.ImageUrls, comment.PostId.ToString(), Forder.SocialComment
                    , _storageConfig.SocialPostContainer);
                comment.Content = ReplaceContent.ReplaceImageUrls(comment.Content, request.ImageUrls, imageUrls);

                comment.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls);
            }

            if (request.VideoUrls?.Any() == true)
            {
                var videoUrls = await _imageService.CopyFileToStorageContainerAsync(request.VideoUrls, comment.PostId.ToString(), Forder.SocialComment
                    , _storageConfig.SocialPostContainer);
                comment.Content = ReplaceContent.ReplaceImageUrls(comment.Content, request.VideoUrls, videoUrls);

                comment.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls);
            }

            _unitOfWork.CommentRepository.Add(comment);

            await _unitOfWork.CommitAsync();

            return comment.Id;
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
                    _loggedInUserService.UserId,
                    isApproval);
            }

            return await _unitOfWork.CommentRepository.ChangeStatusAsync(request.Id, request.PostStatusId,
                _loggedInUserService.UserId,
                isApproval);
        }

        #region Private functions
        private void CheckLoginSession()
        {
            if (string.IsNullOrEmpty(_loggedInUserService.UserId))
            {
                throw new Exception("Must login to take this action");
            }
        }

        private async Task<bool> HasApprovePermission()
        {
            var currentUser = await _authenticationService.GetUserInfoByUserNameAsync(_loggedInUserService.UserId);
            return currentUser.Roles.Contains("SuperAdmin"); //Need improve after assigning post moderation to user groups. Current default is SuperAdmin
        }
        #endregion
    }
}
