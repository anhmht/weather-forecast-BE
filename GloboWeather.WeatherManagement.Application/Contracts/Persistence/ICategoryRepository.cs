using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<Category> AddAsync(Category category);
        Task<int> UpdateAsync(Category entity);
        Task<int> DeleteAsync(Category entity);
    }
}