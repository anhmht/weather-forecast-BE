using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManegement.Application.Contracts;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.SharePost
{
    public class SharePostCommandHandler : IRequestHandler<SharePostCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _loginUserName;
        public SharePostCommandHandler(IUnitOfWork unitOfWork
            , ILoggedInUserService loggedInUserService)
        {
            _unitOfWork = unitOfWork;
            _loginUserName = loggedInUserService.UserId;
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

            var sharePostEntry = new Domain.Entities.Social.SharePost
            {
                Id = Guid.NewGuid(),
                PostId = request.PostId,
                ShareTo = request.ShareTo
            };

            _unitOfWork.SharePostRepository.Add(sharePostEntry);
            await _unitOfWork.CommitAsync();

            return sharePostEntry.Id;
        }
    }
}
