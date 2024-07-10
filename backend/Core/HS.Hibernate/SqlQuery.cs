using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public abstract class SqlQuery<TResultado> : Query<TResultado>, IQuery<TResultado>
    {
        public SqlQuery(ISessionFactory sessionFactory) : base(sessionFactory) { }

        protected abstract string GetSql();
        protected override IQuery CreateQuery()
        {
            return Session.CreateSQLQuery(GetSql());
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
