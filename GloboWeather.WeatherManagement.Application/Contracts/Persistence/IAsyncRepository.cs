using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GloboWeather.WeatherManegement.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> where, CancellationToken token = default, string[] includes = null);
        
        DbSet<T> Db { get; }
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(string[] includes);
        IQueryable<T> GetWhereQuery(Expression<Func<T, bool>> where, string[] includes = null);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteById(string id);
        void DeleteRange(IEnumerable<T> entities);
        void DeleteWhere(Expression<Func<T, bool>> where);
        Task<int> CountAsync(Expression<Func<T, bool>> where);
        IQueryable<T> GetAllQuery(string[] includes = null);
        bool Contains(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
    }
}