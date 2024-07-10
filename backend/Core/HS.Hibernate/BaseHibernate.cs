using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public abstract class BaseHibernate
    {
        private ISession _session = null;

        public BaseHibernate(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        protected ISessionFactory SessionFactory { get; }

        protected ISession Session => _session ?? (_session = SessionFactory.GetCurrentSession());
    }
}
