using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public abstract class SqlQueryList<TResultado>: SqlQuery<IList<TResultado>>, IQuery<IList<TResultado>>
    {
        public SqlQueryList(ISessionFactory sessionFactory) : base(sessionFactory) { }

        protected override IList<TResultado> InnerExecute(IQuery query)
        {
            return query.List<TResultado>();
        }
    }
}
