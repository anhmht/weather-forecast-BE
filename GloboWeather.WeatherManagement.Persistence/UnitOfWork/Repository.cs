using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Persistence.UnitOfWork
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly GloboWeatherDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private bool _tracking;

        public DbSet<T> Db => _dbSet;

        public Repository(GloboWeatherDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _tracking = true;
        }

        public Repository<T> WithTracking()
        {
            _tracking = true;
            return this;
        }
        public Repository<T> WithoutTracking()
        {
            _tracking = false;
            return this;
        }

        public virtual async Task<T> GetAsync(string id) => await _dbSet.FindAsync(id);

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
            => _tracking ? await _dbSet.FirstOrDefaultAsync(predicate)
                : await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);

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
            if (_tracking)
            {
                return await _dbSet.ToListAsync();
            }
            else
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> where, string[] includes = null)
            => await GetWhereQuery(where, includes).ToListAsync();

        public virtual IQueryable<T> GetWhereQuery(Expression<Func<T, bool>> where, string[] includes = null)
        {
            var query = _dbSet.Where(where);
            if (includes != null && includes.Any())
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            if (_tracking)
            {
                return query;
            }
            return query.AsNoTracking();
        }

        //public virtual void AddAsync(T entity)
        //{
        //    _dbSet.AddAsync(entity);
        //}

        //public virtual void AddRangeAsync(IEnumerable<T> entities)
        //{
        //    _dbSet.AddRangeAsync(entities);
        //}

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

        public IQueryable<T> GetAllQuery(string[] includes = null)
        {
            if (includes != null && includes.Any())
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query;
            }

            if (_tracking)
            {
                return _dbSet;
            }

            return _dbSet.AsNoTracking();
        }

        //public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression, string[] includes = null)
        //{
        //    if (includes != null && includes.Any())
        //    {
        //        var query = _dbContext.Set<T>().Include(includes.First());
        //        foreach (var include in includes.Skip(1))
        //            query = query.Include(include);
        //        return await query.FirstOrDefaultAsync(expression);
        //    }

        //    if (_tracking)
        //    {
        //        return await _dbSet.FirstOrDefaultAsync(expression);
        //    }
        //    else
        //    {
        //        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
        //    }
        //}

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate) > 0;
        }
    }
}
