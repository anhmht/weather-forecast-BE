using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Weather.IRepository
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
    }
}