using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<bool> ChangeStatusAsync(Guid id, int postStatusId, string userName, bool isApproval);
    }
}