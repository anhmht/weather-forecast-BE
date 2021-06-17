using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.UnitOfWork
{
    public interface IRepository<T> where T : class, new()
    {
        public DbSet<T> Db { get; }
        public Repository<T> WithTracking();
        public Repository<T> WithoutTracking();
        public Task<T> GetAsync(string id);
        public Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllAsync(string[] includes);
        public Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> where, string[] includes = null);
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
