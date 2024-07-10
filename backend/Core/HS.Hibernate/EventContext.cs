using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class EventContext : IEventContext
    {
        private readonly ISessionFactory _factory;
        private readonly ISession _session;
        private readonly ITransaction _transaction;

        public EventContext(ISessionFactory sessionFactory)
        {
            _factory = sessionFactory;
            _session = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(_session);
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            CurrentSessionContext.Unbind(_factory);
            _transaction.Dispose();
            _session.Dispose();
        }
    }
}
