using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly GloboWeatherDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
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

        public async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> where, string[] includes = null)
            => await GetWhereQuery(where, includes).ToListAsync();

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

    }
}