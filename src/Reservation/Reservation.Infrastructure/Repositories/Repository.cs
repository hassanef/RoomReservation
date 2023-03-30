using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reservation.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public Repository(DbContext dbContext, IHttpContextAccessor httpContextAccessor = null)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public virtual void Create(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.Set<T>().Add(item);

            this.SaveChanges();
        }


        public virtual async Task<T> CreateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            await _dbContext.Set<T>().AddAsync(item);

            await this.SaveChangesAsync();

            return item;
        }

        public virtual async Task<T> CreateAsyncUoW(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            await _dbContext.Set<T>().AddAsync(item);

            return item;
        }


        public virtual void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.Entry(item).State = EntityState.Modified;

            this.SaveChanges(); 
        }
        public virtual async Task<T> UpdateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.Entry(item).State = EntityState.Modified;

            await this.SaveChangesAsync();

            return item;
        }

        public virtual T UpdateUoW(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _dbContext.Set<T>().Update(item);

            return item;
        }
        public virtual void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
            }
        }


        public virtual async System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

   

        public virtual bool DeleteUoW(T item)
        {
            _dbContext.Set<T>().Remove(item);
            return true;
        }
        public virtual void Delete(T item)
        {
            _dbContext.Set<T>().Attach(item);
            _dbContext.Set<T>().Remove(item);
            this.SaveChanges(); 
        }
        public virtual async Task DeleteAsync(T item)
        {
            _dbContext.Set<T>().Attach(item);
            _dbContext.Set<T>().Remove(item);

            await this.SaveChangesAsync();
        }
        public virtual IQueryable<T> FetchMulti(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbContext.Set<T>().AsNoTracking() :
                 _dbContext.Set<T>().AsNoTracking().Where(predicate);
        }

        public virtual IQueryable<T> FetchMultiWithTracking(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbContext.Set<T>() :
                 _dbContext.Set<T>().Where(predicate);
        }

        public void RemoveEntitiesUoW<S>(IEnumerable<S> entities) where S : class
        {
            foreach (var entity in entities)
            {
                _dbContext.Remove<S>(entity);
            }

        }

        public virtual Boolean Any(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().Any(predicate);
        }

        public virtual async Task<Boolean> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().AnyAsync(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbContext.Set<T>().FirstOrDefault() : _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await _dbContext.Set<T>().FirstOrDefaultAsync() : await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual T FirstOrDefaultWithReload(Expression<Func<T, bool>> predicate)
        {
            var entity = _dbContext.Set<T>().FirstOrDefault(predicate);
            if (entity == null)
                return default(T);
            _dbContext.Entry(entity).Reload();
            return entity;
        }

        public virtual async Task<T> LastOrDefaultWithReloadAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbContext.Set<T>().LastOrDefaultAsync(predicate);
            if (entity == null)
                return default(T);
            _dbContext.Entry(entity).Reload();
            return entity;
        }

        public virtual T LastOrDefault(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? _dbContext.Set<T>().LastOrDefault() : _dbContext.Set<T>().LastOrDefault(predicate);
        }

        public virtual async Task<T> FirstOrDefaultWithReloadAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
            if (entity == null)
                return default(T);
            _dbContext.Entry(entity).Reload();
            return entity;
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().SingleOrDefault(predicate);
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? await _dbContext.Set<T>().SingleOrDefaultAsync() : await _dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ?
            _dbContext.Set<T>().Count() :
            _dbContext.Set<T>().Count(predicate);

        }
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await _dbContext.Set<T>().CountAsync() : await _dbContext.Set<T>().CountAsync(predicate);
        }
        public async Task BeginTran()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTran()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }
        public async Task RollBack()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
