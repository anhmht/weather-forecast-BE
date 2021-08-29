using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManegement.Application.Contracts;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetUnReadCountSocialNotification
{
    public class GetUnreadCountSocialNotificationHandle : IRequestHandler<GetUnreadCountSocialNotificationQuery, GetUnreadCountSocialNotificationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _loginUserName;

        public GetUnreadCountSocialNotificationHandle(IUnitOfWork unitOfWork, ILoggedInUserService loggedInUserService)
        {
            _unitOfWork = unitOfWork;
            _loginUserName = loggedInUserService.UserId;
        }

        public async Task<GetUnreadCountSocialNotificationResponse> Handle(
            GetUnreadCountSocialNotificationQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_loginUserName))
            {
                throw new Exception("Must login to take this action");
            }

            var unReadNotificationCount =
                await _unitOfWork.SocialNotificationRepository.CountAsync(x =>
                    x.IsRead == false & x.Receiver == _loginUserName);

            return new GetUnreadCountSocialNotificationResponse {Count = unReadNotificationCount};
        }
    }
}
