using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification
{
    public class GetListSocialNotificationQuery : BasePagingRequest, IRequest<GetListSocialNotificationResponse>
    {
        /// <summary>
        /// True: get read notification, False: get un-read notification; NULL: get all
        /// </summary>
        public bool? IsRead { get; set; }
    }
}
