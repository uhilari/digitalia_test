using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class DataReader : BaseHibernate, IDataReader
    {
        public DataReader(ISessionFactory sessionFactory) : base(sessionFactory) { }

        public TEntity Get<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = Session.Get<TEntity>(id);
            if (entity == null)
                throw new NotFoundEntityException();
            if (entity != null && entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = await Session.GetAsync<TEntity>(id);
            if (entity == null)
                throw new NotFoundEntityException();
            if (entity != null && entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public TEntity GetOrNull<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = Session.Get<TEntity>(id);
            if (entity != null && entity.Eliminado)
                return null;
            return entity;
        }

        public async Task<TEntity> GetOrNullAsync<TEntity>(Guid id) where TEntity : Entity
        {
            var entity = await Session.GetAsync<TEntity>(id);
            if (entity != null && entity.Eliminado)
                return null;
            return entity;
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where(expresion));
            var entity = criteria.UniqueResult<TEntity>();
            if (entity == null)
                throw new NotFoundEntityException();
            if (entity != null && entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where(expresion));
            var entity = await criteria.UniqueResultAsync<TEntity>();
            if (entity == null)
                throw new NotFoundEntityException();
            if (entity != null && entity.Eliminado)
                throw new NotFoundEntityException();
            return entity;
        }

        public TEntity GetOrNull<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where(expresion));
            var entity = criteria.UniqueResult<TEntity>();
            if (entity != null && entity.Eliminado)
                return null;
            return entity;
        }

        public async Task<TEntity> GetOrNullAsync<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where(expresion));
            var entity = await criteria.UniqueResultAsync<TEntity>();
            if (entity != null && entity.Eliminado)
                return null;
            return entity;
        }

        public IList<TEntity> List<TEntity>() where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where<TEntity>(c => c.Eliminado == false));
            return criteria.List<TEntity>();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity>(string orden = null) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where<TEntity>(c => c.Eliminado == false));
            if (!string.IsNullOrEmpty(orden))
                criteria.AddOrder(new Order(orden, true));
            return await criteria.ListAsync<TEntity>();
        }

        public IList<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expresion) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where<TEntity>(expresion));
            criteria.Add(Restrictions.Where<TEntity>(c => c.Eliminado == false));
            return criteria.List<TEntity>();
        }

        public async Task<IList<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>> expresion, string orden = null) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(Restrictions.Where<TEntity>(expresion));
            criteria.Add(Restrictions.Where<TEntity>(c => c.Eliminado == false));
            if (!string.IsNullOrEmpty(orden))
                criteria.AddOrder(new Order(orden, true));
            return await criteria.ListAsync<TEntity>();
        }

        public async Task<IList<TEntity>> ContainsAsync<TEntity, S>(Expression<Func<TEntity, S>> expression, S[] valores, string orden = null) where TEntity : Entity
        {
            var criteria = Session.CreateCriteria<TEntity>();
            var propName = expression.GetPropName();
            criteria.Add(Restrictions.In(propName, valores));
            criteria.Add(Restrictions.Where<TEntity>(c => c.Eliminado == false));
            if (!string.IsNullOrEmpty(orden))
                criteria.AddOrder(new Order(orden, true));
            return await criteria.ListAsync<TEntity>();
        }
    }
}
