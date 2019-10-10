using AutoMapper;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server_WEB_Programming.Lab2.Dal.UoW.Implementations.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected readonly IMapper Mapper;

        public Repository(DbContext context, IMapper mapper)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            Mapper = mapper;
        }

        public async Task CreateAsync(TEntity entity)
        {
            DbSet.Add(entity);

            await Task.CompletedTask;
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(TKey id)
        {
            var entityToDelete = await DbSet.FindAsync(id);

            //if (entityToDelete == null)
            //{
            //    throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Entity with id: {id} not found when trying to update entity. Entity was no Deleted.");
            //}

            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }
    }
}
