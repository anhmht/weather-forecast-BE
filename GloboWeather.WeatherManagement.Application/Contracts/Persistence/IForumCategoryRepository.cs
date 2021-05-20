using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IForumCategoryRepository : IAsyncRepository<ForumCategory>
    {
        Task<List<ForumCategory>> GetForumCategoriesAsync();
        Task<List<ForumCategory>> GetForumCategoriesAsync(Guid forumId);
        Task<ForumCategory> GetForumCategory(Guid categoryId);
        Task<bool> CreateCategoryAsync(ForumCategory newCategory);
        Task<bool> UpdateCategoryAsync(ForumCategory editCategory);
        Task<bool> DeleteCategoryAsync(Guid categoryId);

    }
}