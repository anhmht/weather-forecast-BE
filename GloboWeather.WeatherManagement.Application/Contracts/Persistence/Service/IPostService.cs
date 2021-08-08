using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken);
    }
}
