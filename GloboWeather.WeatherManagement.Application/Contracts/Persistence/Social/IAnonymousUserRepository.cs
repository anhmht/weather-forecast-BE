using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social
{
    public interface IAnonymousUserRepository : IAsyncRepository<AnonymousUser>
    {
        /// <summary>
        /// Save without commit to database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Guid> Save(AnonymousUserRequest request);
        Task<Guid> SaveAsync(AnonymousUserRequest request, CancellationToken cancellationToken);
    }
}