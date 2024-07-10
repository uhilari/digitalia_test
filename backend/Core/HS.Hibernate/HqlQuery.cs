using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public abstract class HqlQuery<TResultado> : Query<TResultado>, IQuery<TResultado>
    {
        public HqlQuery(ISessionFactory sessionFactory) : base(sessionFactory) { }

        protected abstract string GetHql();

        protected override IQuery CreateQuery()
        {
            return Session.CreateQuery(GetHql());
        }

        protected override TResultado InnerExecute(IQuery query)
        {
            return query.UniqueResult<TResultado>();
        }

        protected override Task<TResultado> InnerExecuteAsync(IQuery query)
        {
            return query.UniqueResultAsync<TResultado>();
        }
    }
}
