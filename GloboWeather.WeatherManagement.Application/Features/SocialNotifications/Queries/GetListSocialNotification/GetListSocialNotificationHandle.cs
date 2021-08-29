using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManegement.Application.Contracts;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification
{
    public class GetListSocialNotificationHandle : IRequestHandler<GetListSocialNotificationQuery, GetListSocialNotificationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _loginUserName;
        private readonly IAuthenticationService _authenticationService;
        public GetListSocialNotificationHandle(IUnitOfWork unitOfWork, IMapper mapper
            , ILoggedInUserService loggedInUserService, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loginUserName = loggedInUserService.UserId;
            _authenticationService = authenticationService;
        }

        public async Task<GetListSocialNotificationResponse> Handle(GetListSocialNotificationQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_loginUserName))
            {
                throw new Exception("Must login to take this action");
            }

            var listPaging = await _unitOfWork.SocialNotificationRepository
                .GetWhereQuery(x => x.Receiver == _loginUserName && (request.IsRead == null || request.IsRead == x.IsRead))
                .OrderByDescending(x => x.CreateDate)
                .PaginateAsync(request.Page, request.Limit, cancellationToken);

            var users = await _authenticationService.GetAllUserAsync(true);
            var anonymousUserIds = listPaging.Items
                .Where(x => x.AnonymousUserId.HasValue && !x.AnonymousUserId.Equals(Guid.Empty))
                .Select(x => x.AnonymousUserId).ToList();
            var anonymousUsers =
                (await _unitOfWork.AnonymousUserRepository.GetWhereAsync(x => anonymousUserIds.Contains(x.Id),
                    cancellationToken)).ToList();
            var socialNotifications = new List<SocialNotificationVm>();
            if (listPaging.Items.Any())
            {
                socialNotifications = _mapper.Map<List<SocialNotificationVm>>(listPaging.Items);
                foreach (var socialNotificationVm in socialNotifications)
                {
                    if (!string.IsNullOrEmpty(socialNotificationVm.CreateBy))
                    {
                        var user = users.Find(x => x.UserName == socialNotificationVm.CreateBy);
                        if (user != null)
                        {
                            socialNotificationVm.FromUserAvatar = user.AvatarUrl;
                            socialNotificationVm.FromUserFullName = user.FullName;
                            socialNotificationVm.FromUserShortName = user.ShortName;
                        }
                    }
                    else if (socialNotificationVm.AnonymousUserId.HasValue && !socialNotificationVm.AnonymousUserId.Equals(Guid.Empty))
                    {
                        var anonymousUser = anonymousUsers.Find(x => x.Id == socialNotificationVm.AnonymousUserId);
                        if (anonymousUser != null)
                        {
                            socialNotificationVm.FromUserFullName = anonymousUser.FullName;
                            socialNotificationVm.FromUserShortName = string.Join("",
                                anonymousUser.FullName.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.First()));
                        }
                    }
                }
            }

            var response = new GetListSocialNotificationResponse
            {
                CurrentPage = listPaging.CurrentPage,
                TotalItems = listPaging.TotalItems,
                TotalPages = listPaging.TotalPages,
                Items = socialNotifications
            };

            return response;
        }
    }
}
