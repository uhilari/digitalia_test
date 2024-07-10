using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public abstract class Query<TResultado> : BaseHibernate, IQuery<TResultado>
    {
        public Query(ISessionFactory sessionFactory) : base(sessionFactory) { }

        protected abstract IQuery CreateQuery();
        protected virtual TResultado InnerExecute(IQuery query) => default(TResultado);
        protected virtual Task<TResultado> InnerExecuteAsync(IQuery query) => Task.FromResult(default(TResultado));
        protected virtual void SetParameter(IQuery query) { }
        protected virtual IResultTransformer GetTransformer() => null;

        public virtual TResultado Execute()
        {
            var query = CreateQuery();
            SetParameter(query);
            var transformer = GetTransformer();
            if (transformer != null)
                query.SetResultTransformer(transformer);
            return InnerExecute(query);
        }

        public virtual async Task<TResultado> ExecuteAsync()
        {
            var query = CreateQuery();
            SetParameter(query);
            var transformer = GetTransformer();
            if (transformer != null)
                query.SetResultTransformer(transformer);
            return await InnerExecuteAsync(query);
        }
    }
}
