using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IForumRepository : IAsyncRepository<Forum>
    {
        Task<List<Forum>> GetForumsAsync();
        Task<Forum> GetForumAsync(Guid id);
        Task<bool> CreateForumAsync(Forum newForum);
        Task<bool> UpdateForumAsync(Forum editedForum);
        Task<bool> DeleteForumAsync(Guid forumId);
        
    }
}