using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Commands.UpdateIsReadSocialNotification
{
    public class UpdateIsReadSocialNotificationCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
