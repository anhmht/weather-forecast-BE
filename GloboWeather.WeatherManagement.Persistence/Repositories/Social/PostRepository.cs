using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Social;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Persistence.Repositories.Social
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangeStatusAsync(Guid id, int postStatusId, string userName, bool isApproval)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(id);
            if (post == null)
            {
                throw new NotFoundException("Post", id);
            }

            post.StatusId = postStatusId;
            if (post.StatusId == (int) PostStatus.Public)
            {
                post.PublicDate = DateTime.Now;
            }

            if (isApproval)
            {
                post.ApprovedByUserName = userName;
            }
            _unitOfWork.PostRepository.Update(post);
            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}