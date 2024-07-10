using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
  public abstract class HqlQueryList<TResultado>: HqlQuery<IList<TResultado>>, IQuery<IList<TResultado>>
  {
    public HqlQueryList(ISessionFactory sessionFactory) : base(sessionFactory) { }

    protected override IList<TResultado> InnerExecute(IQuery query)
    {
      return query.List<TResultado>();
    }
  }
}
