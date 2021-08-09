using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Persistence.Repositories.Social
{
    public class AnonymousUserRepository : BaseRepository<AnonymousUser>, IAnonymousUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnonymousUserRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Save without Commit to database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Guid> Save(AnonymousUserRequest request)
        {
            AnonymousUser anonymousUser = null;
            if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Phone))
            {
                anonymousUser = await _unitOfWork.AnonymousUserRepository.FindAsync(x =>
                    x.Email.ToLower() == request.Email.ToLower().Trim()
                    && x.Phone == request.Phone.Trim());
            }

            if (anonymousUser != null)
            {
                if (!anonymousUser.FullName.ToLower().Equals(request.FullName.ToLower().Trim()))
                {
                    anonymousUser.FullName = request.FullName.Trim();
                    _unitOfWork.AnonymousUserRepository.Update(anonymousUser);
                }
            }
            else
            {
                anonymousUser = new AnonymousUser
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FullName.Trim(),
                    Email = request.Email.Trim(),
                    Phone = request.Phone.Trim(),
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.AnonymousUserRepository.Add(anonymousUser);
            }

            return anonymousUser.Id;
        }

        public async Task<Guid> SaveAsync(AnonymousUserRequest request, CancellationToken cancellationToken)
        {
            var anonymousUser = await _unitOfWork.AnonymousUserRepository.FindAsync(x =>
                x.Email.ToLower() == request.Email.ToLower().Trim()
                && x.Phone == request.Phone.Trim());
            if (anonymousUser != null)
            {
                if (!anonymousUser.FullName.ToLower().Equals(request.FullName.ToLower().Trim()))
                {
                    anonymousUser.FullName = request.FullName.Trim();
                    _unitOfWork.AnonymousUserRepository.Update(anonymousUser);
                    await _unitOfWork.CommitAsync();
                }
            }
            else
            {
                anonymousUser = new AnonymousUser
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FullName.Trim(),
                    Email = request.Email.Trim(),
                    Phone = request.Phone.Trim(),
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.AnonymousUserRepository.Add(anonymousUser);
                await _unitOfWork.CommitAsync();
            }

            return anonymousUser.Id;
        }
    }
}