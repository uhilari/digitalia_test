using HS.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class DataReader : BaseEntityFramework, IDataReader
    {
        public DataReader(IContextFactory contextFactory): base(contextFactory) { }

        public TEntity Get<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = DbContext.Set<TEntity>().Find(id) ?? throw new NotFoundEntityException();
            if (entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var entity = DbContext.Set<TEntity>().First(expresion) ?? throw new NotFoundEntityException();
            if (entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = (await DbContext.Set<TEntity>().FindAsync(id)) ?? throw new NotFoundEntityException();
            if (entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var entity = await DbContext.Set<TEntity>().FirstAsync(expresion) ?? throw new NotFoundEntityException();
            if (entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public TEntity? GetOrNull<TEntity>(Guid id) where TEntity : Entity
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public TEntity? GetOrNull<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            return DbContext.Set<TEntity>().First(expresion);
        }

        public async Task<TEntity?> GetOrNullAsync<TEntity>(Guid id) where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetOrNullAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().FirstAsync(expresion);
        }

        public IList<TEntity> List<TEntity>() where TEntity : Entity
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            return DbContext.Set<TEntity>().Where(expresion).ToList();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity>() where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> orden) where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().OrderBy(orden).ToListAsync();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().Where(expresion).ToListAsync();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> expresion, Expression<Func<TEntity, TKey>> orden) where TEntity : Entity
        {
            return await DbContext.Set<TEntity>().Where(expresion).OrderBy(orden).ToListAsync();
        }
    }
}
