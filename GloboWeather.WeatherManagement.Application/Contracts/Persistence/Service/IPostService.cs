using System;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostCommand request, CancellationToken cancellationToken);
        Task<Guid> CreateCommentAsync(CreateCommentCommand request, CancellationToken cancellationToken);
        Task<bool> ChangeStatusAsync(ChangeStatusCommand request, CancellationToken cancellationToken);
    }
}
