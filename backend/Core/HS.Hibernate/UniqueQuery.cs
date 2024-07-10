using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS.Hibernate
{
    public class UniqueQuery<TEntity, TUniqueKey> : BaseHibernate, IUniqueQuery<TEntity, TUniqueKey>
        where TEntity : Entity
    {
        public UniqueQuery(ISessionFactory sessionFactory)
            : base(sessionFactory) { }

        public TUniqueKey Key { get; set; }
        protected Expression<Func<TEntity, bool>> Condition { get; set; }

        public bool Execute()
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(NHibernate.Criterion.Expression.Where(Condition));
            criteria.Add(NHibernate.Criterion.Expression.Where<TEntity>(t => t.Eliminado == false));
            criteria.SetProjection(NHibernate.Criterion.Projections.RowCount());
            var count = criteria.UniqueResult<int>();
            return count > 0;
        }

        public async Task<bool> ExecuteAsync()
        {
            var criteria = Session.CreateCriteria<TEntity>();
            criteria.Add(NHibernate.Criterion.Expression.Where(Condition));
            criteria.Add(NHibernate.Criterion.Expression.Where<TEntity>(t => t.Eliminado == false));
            criteria.SetProjection(NHibernate.Criterion.Projections.RowCount());
            var count = await criteria.UniqueResultAsync<int>();
            return count > 0;
        }
    }
}
