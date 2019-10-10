using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server_WEB_Programming.Lab2.Dal.UoW.Interfaces.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true);
        Task UpdateAsync(TEntity entity);
    }
}
