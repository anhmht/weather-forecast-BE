using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly GloboWeatherDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public DbSet<T> Db => _dbSet;

        public BaseRepository(GloboWeatherDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size)
        {
            return await _dbSet.Skip((page - 1) * size).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task UpdateRangeAsync(List<T> entities)
        {
            entities.ForEach(entity => { _dbContext.Entry(entity).State = EntityState.Modified; });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> where, CancellationToken token = default, string[] includes = null)
            => await GetWhereQuery(where, includes).ToListAsync(token);

        public virtual IQueryable<T> GetWhereQuery(Expression<Func<T, bool>> where, string[] includes = null)
        {
            var query = _dbSet.Where(where);
            if (includes != null && includes.Any())
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            
            return query;
        }

        public IQueryable<T> GetAllQuery(string[] includes = null)
        {
            if (includes != null && includes.Any())
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query;
            }

            return _dbSet;
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> GetAllAsync() => await GetAllAsync(null);
        public async Task<IEnumerable<T>> GetAllAsync(string[] includes)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Any())
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return await query.ToListAsync();
            }
            return await _dbSet.ToListAsync();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        //public virtual void Update(T entity)
        //{
        //    _dbSet.Attach(entity);
        //    _dbContext.Entry(entity).State = EntityState.Modified;
        //}

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void DeleteById(string id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> where)
        {
            var objects = _dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.CountAsync(where);
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate) > 0;
        }
    }
}