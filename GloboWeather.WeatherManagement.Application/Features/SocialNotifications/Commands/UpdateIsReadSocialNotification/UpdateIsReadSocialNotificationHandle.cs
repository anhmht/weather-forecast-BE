using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Commands.UpdateIsReadSocialNotification
{
    public class UpdateIsReadSocialNotificationHandle : IRequestHandler<UpdateIsReadSocialNotificationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _loginUserName;
        private readonly IAuthenticationService _authenticationService;
        public UpdateIsReadSocialNotificationHandle(IUnitOfWork unitOfWork, IMapper mapper
            , ILoggedInUserService loggedInUserService, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loginUserName = loggedInUserService.UserId;
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(UpdateIsReadSocialNotificationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateIsReadSocialNotificationCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var notification = await _unitOfWork.SocialNotificationRepository.GetByIdAsync(request.Id);

            if (notification == null)
            {
                throw new NotFoundException(nameof(SocialNotification), request.Id);
            }

            notification.IsRead = true;
            _unitOfWork.SocialNotificationRepository.Update(notification);
            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}
