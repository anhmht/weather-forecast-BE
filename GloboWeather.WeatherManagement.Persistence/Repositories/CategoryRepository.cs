using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> AddAsync(Category category)
        {
            _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        public async Task<int> UpdateAsync(Category entity)
        {
            _unitOfWork.CategoryRepository.Update(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> DeleteAsync(Category entity)
        {
            _unitOfWork.CategoryRepository.Delete(entity);
            return await _unitOfWork.CommitAsync();
        }
    }
}