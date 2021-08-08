using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
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
            if (string.IsNullOrEmpty(_loggedInUserService.UserId))
            {
                throw new Exception("Must login to create a post");
            }

            var post = new Post
            {
                Id = Guid.NewGuid(), Content = request.Content, StatusId = (int) PostStatus.WaitingForApproval
            };

            if (request.ImageUrls?.Any() == true)
            {
                var imageUrls = await _imageService.CopyFileToStorageContainerAsync(request.ImageUrls, post.Id.ToString(), Forder.Post
                    , _storageConfig.SocialPostContainer);
                post.Content = ReplaceContent.ReplaceImageUrls(post.Content, request.ImageUrls, imageUrls);

                post.ImageUrls = string.Join(Constants.SemiColonStringSeparator, imageUrls);
            }

            if (request.VideoUrls?.Any() == true)
            {
                var videoUrls = await _imageService.CopyFileToStorageContainerAsync(request.VideoUrls, post.Id.ToString(), Forder.Post
                    , _storageConfig.SocialPostContainer);
                post.Content = ReplaceContent.ReplaceImageUrls(post.Content, request.VideoUrls, videoUrls);

                post.VideoUrls = string.Join(Constants.SemiColonStringSeparator, videoUrls);
            }

            _unitOfWork.PostRepository.Add(post);

            await _unitOfWork.CommitAsync();

            return post.Id;
        }
    }
}
