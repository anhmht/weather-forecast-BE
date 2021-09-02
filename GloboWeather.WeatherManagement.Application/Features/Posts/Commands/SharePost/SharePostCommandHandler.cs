using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.SignalR;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.SharePost
{
    public class SharePostCommandHandler : IRequestHandler<SharePostCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _loginUserName;
        private readonly IHubContext<NotificationHub> _hubClient;
        public SharePostCommandHandler(IUnitOfWork unitOfWork
            , ILoggedInUserService loggedInUserService
            , IHubContext<NotificationHub> hubClient)
        {
            _unitOfWork = unitOfWork;
            _loginUserName = loggedInUserService.UserId;
            _hubClient = hubClient;
        }

        public async Task<Guid> Handle(SharePostCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_loginUserName))
            {
                throw new Exception("Must login to take this action");
            }

            var validator = new SharePostCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new NotFoundException(nameof(Post), request.PostId);
            }

            var sharePostEntry = new Domain.Entities.Social.SharePost
            {
                Id = Guid.NewGuid(),
                PostId = request.PostId,
                ShareTo = request.ShareTo
            };

            _unitOfWork.SharePostRepository.Add(sharePostEntry);

            await _unitOfWork.CommitAsync();

            if (_loginUserName != post.CreateBy)
            {
                await PushNotification(post.CreateBy, post.Id, NotificationAction.SharePost);
            }

            return sharePostEntry.Id;
        }
        private async Task PushNotification(string receiver, Guid? postId, string action
            , string description = "")
        {
            var notification = new SocialNotification
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                Action = action,
                Receiver = receiver,
                Description = description,
                CreateBy = _loginUserName,
                CreateDate = DateTime.Now
            };

            _unitOfWork.SocialNotificationRepository.Add(notification);

            try
            {
                await _hubClient.Clients.Group(receiver).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(notification));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw new Exception($"Need write log when push notification error{Environment.NewLine}{e}");
            }
        }
    }
}
