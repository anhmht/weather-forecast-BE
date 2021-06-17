using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IForumRepository : IAsyncRepository<Forum>
    {
        
    }
}