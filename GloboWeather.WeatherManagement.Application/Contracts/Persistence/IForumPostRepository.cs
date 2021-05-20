using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IForumPostRepository : IAsyncRepository<ForumCategory>
    {
        Task<List<ForumPost>> GetForumPostsAsync(Guid topicId);
        Task<List<ForumPost>> GetApprovedForumPostsAsync(Guid topicId);
        Task<ForumPost> GetForumPostAsync(Guid postId);
        Task<bool> AddNewPostAsync(ForumPost newPost);
        Task<bool> UpdatePostAsync(ForumPost editedPost);
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> SetPostAnswerStatusAsync(Guid postId, bool isAnswer);
        Task<bool> MarkUserPostsAsDeletedAsync(Guid userId);
    }
}