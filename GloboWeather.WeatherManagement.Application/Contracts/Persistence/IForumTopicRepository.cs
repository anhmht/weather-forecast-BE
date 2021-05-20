using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IForumTopicRepository:IAsyncRepository<ForumTopic>
    {
        Task<List<ForumTopic>> GetAllForumTopicAsync(Guid catId);
      //  Task<List<ForumTopic>> GetNewTopicsAsync(int )
    }
}