using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateRangeAsync(List<T> entities);
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> where, CancellationToken token = default, string[] includes = null);
        
        public DbSet<T> Db { get; }
        public Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllAsync(string[] includes);
        public IQueryable<T> GetWhereQuery(Expression<Func<T, bool>> where, string[] includes = null);
        public void Add(T entity);
        public void AddRange(IEnumerable<T> entities);
        public void Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);
        public void Delete(T entity);
        public void DeleteById(string id);
        public void DeleteRange(IEnumerable<T> entities);
        public void DeleteWhere(Expression<Func<T, bool>> where);
        public Task<int> CountAsync(Expression<Func<T, bool>> where);
        public IQueryable<T> GetAllQuery(string[] includes = null);
        public bool Contains(Expression<Func<T, bool>> predicate);
    }
}