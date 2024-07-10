using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IDataReader
    {
        TEntity Get<TEntity>(Guid id) where TEntity : Entity;
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        TEntity GetOrNull<TEntity>(Guid id) where TEntity : Entity;
        TEntity GetOrNull<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        IList<TEntity> List<TEntity>() where TEntity : Entity;
        IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : Entity;
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        Task<TEntity> GetOrNullAsync<TEntity>(Guid id) where TEntity : Entity;
        Task<TEntity> GetOrNullAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        Task<IList<TEntity>> ListAsync<TEntity>() where TEntity : Entity;
        Task<IList<TEntity>> ListAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> orden) where TEntity : Entity;
        Task<IList<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity;
        Task<IList<TEntity>> ListAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> expresion, Expression<Func<TEntity, TKey>> orden) where TEntity : Entity;
    }
}
